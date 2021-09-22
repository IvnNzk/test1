import React from 'react'
import ReactDOM from 'react-dom'

import Routes from './pages/routes'

const App = () => (
  <div className={['App']}>
    <Routes />
  </div>
)

ReactDOM.render(
  <App />,
  document.getElementById('root')
)
