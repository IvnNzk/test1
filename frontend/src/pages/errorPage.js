import Paper from '@material-ui/core/Paper'
import { makeStyles } from '@material-ui/core/styles'
import React from 'react'
import { Link, Redirect, useParams } from 'react-router-dom'


const errorParameters = (code) => {
  switch (code) {
    case '404':
      return {
        message: 'Not Found',
        linkBack: {
          url: '/',
          text: 'Upload page'
        }
      }
    case '500':
      return {
        message: 'Server Error',
        linkBack: {
          url: '/',
          text: 'Upload page'
        }
      }
    default:
      return {
        message: 'Undefined',
        linkBack: {
          url: '/',
          text: 'Upload page'
        }
      }
  }
}

const useStyles = makeStyles(() => ({
  root: {
    display: 'flex',
    width: '100vw',
    height: '100vh'
  },
  paper: {
    maxWidth: '767px',
    width: '100%',
    lineHeight: '1.4',
    padding: '110px 40px',
    textAlign: 'center',
    position: 'absolute',
    left: '50%',
    top: '50%',
    transform: 'translate(-50%, -50%)'
  },
  code_number: {
    fontSize: '165px',
    marginBottom: '25px',
    marginTop: '25px'
  },
  coloredNumber: {
    color: '#3f51b5'
  }
}))

const ErrorPage = () => {
  const classes = useStyles()

  const { code } = useParams()

  const param = errorParameters(code)

  if (param.message === 'Undefined') {
    return <Redirect to="/error/404" />
  }

  return (
    <div className={classes.root}>
      <Paper elevation={3} className={classes.paper}>
        <h1 className={classes.code_number}>
          {code}
        </h1>
        <h2>{param.message}</h2>
        <Link to={param.linkBack.url} replace><h6>{param.linkBack.text}</h6></Link>
      </Paper>
    </div>
  )
}

export default ErrorPage
