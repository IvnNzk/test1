import {
  Button, Checkbox, FormControlLabel, FormGroup, Paper, Typography
} from '@material-ui/core'
import Box from '@material-ui/core/Box'
import Grid from '@material-ui/core/Grid'
import axios from 'axios'
import React, { useState } from 'react'

const UploadForm = () => {
  const [file, setFile] = useState(null)
  const [filename, setFilename] = useState('')
  const [message, setMessage] = useState('')

  // checkboxs
  const [containCyrillic, setContainCyrillic] = useState(false)
  const [containsNumbers, setContainsNumbers] = useState(false)
  const [containSpecialCharacters, setSpecialCharacters] = useState(false)
  const [caseSensitivity, setCaseSensitivity] = useState(false)

  const onChange = (e) => {
    setFile(e.target.files[0])
    setFilename(e.target.files[0].name)
  }

  const onSubmit = async (e) => {
    e.preventDefault()

    if (!containCyrillic && !containsNumbers && !containSpecialCharacters) {
      setMessage('Должен содержать специальные символы, числа, или кириллицу')
      return
    }

    if (file === null) {
      setMessage('Файл не выбран')
    }

    console.log('containCyrillic', containCyrillic)
    console.log('containsNumbers', containsNumbers)
    console.log('containsNumbers', containSpecialCharacters)
    console.log('caseSensitivity', caseSensitivity)

    const formData = new FormData()
    formData.append('uploadedFile', file)
    formData.append('containCyrillic', containCyrillic)
    formData.append('containsNumbers', containsNumbers)
    formData.append('containSpecialCharacters', containSpecialCharacters)
    formData.append('caseSensitivity', caseSensitivity)

    axios.post(
      'https://localhost:5001/DatasetUploader',
      formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        },
        params: {
          containCyrillic,
          containsNumbers,
          containSpecialCharacters,
          caseSensitivity
        }
      }
    ).then(
      (response) => {
        console.log(response)
        setMessage(`${filename} был загружен`)
        setFile(null)
        setFilename('')
        return response
      }
    ).catch((error) => setMessage('Ошибка во время загрузки'))
  }

  return (
    <div>
      <Paper>
        <Typography variant="h5" component="h5">
          Форма загрузки
        </Typography>
        <form onSubmit={onSubmit}>
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <FormGroup>
                <FormControlLabel
                  control={<Checkbox />}
                  label="ContainCyrillic"
                  onChange={(e) => setContainCyrillic(e.target.checked)}
                />
                <FormControlLabel
                  control={<Checkbox />}
                  label="ContainsNumbers"
                  onChange={(e) => setContainsNumbers(e.target.checked)}
                />
                <FormControlLabel
                  control={<Checkbox />}
                  label="ContainSpecialCharacters"
                  onChange={(e) => setSpecialCharacters(e.target.checked)}
                />
                <FormControlLabel
                  control={<Checkbox />}
                  label="CaseSensitivity"
                  onChange={(e) => setCaseSensitivity(e.target.checked)}
                />
              </FormGroup>
            </Grid>
            <Box width="500" display="flex" alignItems="space-between">
              <Button
                variant="contained"
                component="label"
              >
                Выбрать файл
                <input
                  type="file"
                  hidden
                  onChange={onChange}
                />
              </Button>
              <br />
              <Button
                type="submit"
                variant="contained"
                color="primary"
              >
                Отправить на сервер
              </Button>
            </Box>
          </Grid>
        </form>
        <br />
        <Typography component="p">{message}</Typography>
      </Paper>
    </div>
  )
}

export default UploadForm
