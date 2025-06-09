import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import WebApp from './WebApp.tsx'
import { AuthProvider } from './auths/AuthContext.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <AuthProvider>
            <WebApp />
        </AuthProvider>
  </StrictMode>,
)
