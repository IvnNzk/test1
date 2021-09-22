import axios from 'axios'
import { useCallback, useEffect, useState } from 'react'

import useLocalStorage from './useLocalStorage'

export default (url) => {
  const baseUrl = 'https://localhost:5001'

  const [isLoading, setIsLoading] = useState(false)
  const [response, setResponse] = useState(null)
  const [error, setError] = useState(null)
  const [options, setOptions] = useState({})
  // const [token] = useLocalStorage('token')
  const [additionalUrl, setAdditionalUrl] = useState('')

  const doFetch = useCallback((opt, additionalUrlArg) => {
    setOptions(opt || {})
    setAdditionalUrl(additionalUrlArg || '')
    setError(null)
    setResponse(null)
    setIsLoading(true)
  }, [])

  useEffect(() => {
    if (!isLoading) {
      return
    }

    /*
      ...{
        headers: {
          Authorization: token ? `Bearer ${token}` : ''
        }
      }
     */
    const requestOptions = {
      ...options
    }

    axios(baseUrl + url + additionalUrl, requestOptions)
      .then((res) => {
        console.log(res)
        setResponse(res.data)
        setIsLoading(false)
      })
      .catch((err) => {
        setError(err.response ? err.response.data : err.message)
        setIsLoading(false)
      })
  }, [isLoading, options, url, additionalUrl])

  return [{
    isLoading,
    response,
    error
  }, doFetch]
}
