import React from 'react'
import { renderRoutes } from 'react-router-config'
import { BrowserRouter as Router } from 'react-router-dom'
import Redirect from 'react-router-dom/es/Redirect'

import ErrorPage from './errorPage'
import UploadPage from './uploadPage'

const Routes = () => {
  const routes = [
    {
      path: '/',
      exact: true,
      component: UploadPage
    },
    {
      path: '/error/:code',
      exact: true,
      component: ErrorPage
    },
    {
      component: () => <Redirect to="/error/404" />
    }
  ]

  return (
    <Router>
      {renderRoutes(routes)}
    </Router>
  )
}

export default Routes
/*

{
          path: '/businessProcess',
          exact: true,
          component: ProtectedRoute(BusinessProcess)

        },
        {
          path: '/dashboard_default',
          exact: true,
          component: ProtectedRoute(DashboardDefault)
        },
        {
          path: '/serverRpa',
          exact: true,
          component: ProtectedRoute(ServerRpa)
        },
        {
          path: '/robotRunner',
          exact: true,
          component: ProtectedRoute(RobotRunner)
        },
        {
          path: '/workstation',
          exact: true,
          component: ProtectedRoute(Workstation)
        },
        {
          path: '/workstationsGroup',
          exact: true,
          component: ProtectedRoute(WorkstationsGroup)
        },
        {
          path: '/workflow',
          exact: true,
          component: ProtectedRoute(Workflow)
        },
        {
          path: '/domainCredential',
          exact: true,
          component: ProtectedRoute(DomainCredential)
        },
        {
          path: '/sensor',
          exact: true,
          component: ProtectedRoute(Sensor)
        },

 */
