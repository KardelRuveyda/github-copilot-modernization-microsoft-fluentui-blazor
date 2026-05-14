param apiAppName string
param allowedOrigin string

resource api 'Microsoft.Web/sites@2024-04-01' existing = {
  name: apiAppName
}

resource webConfig 'Microsoft.Web/sites/config@2024-04-01' = {
  parent: api
  name: 'web'
  properties: {
    cors: {
      allowedOrigins: [
        allowedOrigin
      ]
      supportCredentials: false
    }
  }
}
