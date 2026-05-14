targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment used to generate a short unique hash for resources.')
param environmentName string

@minLength(1)
@description('Primary location for all resources.')
param location string

var abbrs = {
  resourceGroup: 'rg'
  appServicePlan: 'plan'
  appService: 'app'
}

var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = {
  'azd-env-name': environmentName
  workload: 'fluentdemo'
}

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: '${abbrs.resourceGroup}-${environmentName}'
  location: location
  tags: tags
}

module plan 'modules/appservice-plan.bicep' = {
  name: 'plan'
  scope: rg
  params: {
    name: '${abbrs.appServicePlan}-fluentdemo-${resourceToken}'
    location: location
    tags: tags
  }
}

module api 'modules/appservice-api.bicep' = {
  name: 'api'
  scope: rg
  params: {
    name: '${abbrs.appService}-fluentdemo-api-${resourceToken}'
    location: location
    tags: union(tags, { 'azd-service-name': 'api' })
    appServicePlanId: plan.outputs.id
  }
}

module web 'modules/appservice-web.bicep' = {
  name: 'web'
  scope: rg
  params: {
    name: '${abbrs.appService}-fluentdemo-web-${resourceToken}'
    location: location
    tags: union(tags, { 'azd-service-name': 'web' })
    appServicePlanId: plan.outputs.id
    apiBaseUrl: 'https://${api.outputs.defaultHostName}'
  }
}

module apiCors 'modules/appservice-api-cors.bicep' = {
  name: 'api-cors'
  scope: rg
  params: {
    apiAppName: api.outputs.name
    allowedOrigin: 'https://${web.outputs.defaultHostName}'
  }
}

output AZURE_LOCATION string = location
output AZURE_RESOURCE_GROUP string = rg.name
output API_URL string = 'https://${api.outputs.defaultHostName}'
output WEB_URL string = 'https://${web.outputs.defaultHostName}'
