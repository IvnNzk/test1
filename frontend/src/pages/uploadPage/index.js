import {
  AppBar, Checkbox,
  Container,
  CssBaseline,
  Divider, FormControl, FormControlLabel, FormGroup, FormHelperText, FormLabel, Link,
  Table, TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Toolbar,
  Typography
} from '@material-ui/core'
import Paper from '@material-ui/core/Paper'
import React, { useEffect, useState } from 'react'

import useFetch from '../../hooks/useFetch'
import UploadForm from '../../components/uploadForm'


const UploadPage = () => {
  const [{ response, isLoading, error }, doFetch] = useFetch('/Dataset')
  const [datasetList, setDatasetlist] = useState(null)
  /*
      data: {
        username: email,
        password
      }
   */

  useEffect(() => {
    doFetch({
      method: 'get',
      data: {}
    })
  }, [])

  useEffect(() => {
    if (response) {
      console.log(response)
      setDatasetlist(response)
    }
  }, [response])

  return (
    <>
      <CssBaseline />
      <AppBar position="static">
        <Container maxWidth="lg">
          <Toolbar>
            <Typography>Logo</Typography>
          </Toolbar>
        </Container>
      </AppBar>
      <br />
      <br />
      <br />
      <br />
      <Container maxWidth="lg">
        <Divider />

        <Typography variant="h5" gutterBottom component="div">
          Форма загрузки
        </Typography>

        <UploadForm />

        <br/>

        <Typography variant="h5" gutterBottom component="div">
          Таблица загруженных dataset'ов
        </Typography>

        <br/>

        <TableContainer component={Paper}>
          <Table sx={{minWidth: 650}} aria-label="simple table">
            <TableHead>
              <TableRow>
                <TableCell>
                  UUID
                </TableCell>
                <TableCell align="left">
                  Имя
                </TableCell>
                <TableCell align="left">
                  Дата создания
                </TableCell>
                <TableCell align="left">
                  Кириллица
                </TableCell>
                <TableCell align="left">
                  Цифры
                </TableCell>
                <TableCell align="left">
                  Специальные символы
                </TableCell>
                <TableCell align="left">
                  Чувствительность к регистру
                </TableCell>
                <TableCell align="left">
                  Ответы на изображения (отсутствует, в именах файлов, в
                  отдельном файле)
                </TableCell>
                <TableCell align="left">
                  Ссылка на скачивание
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {!isLoading && datasetList ? datasetList.map((row) => (
                <TableRow
                  key={row.id}
                  sx={{'&:last-child td, &:last-child th': {border: 0}}}
                >
                  <TableCell component="th" scope="row">
                    {row.id}
                  </TableCell>
                  <TableCell align="right">{row.fileName}</TableCell>
                  <TableCell align="right">{row.createdAt}</TableCell>
                  <TableCell align="right">{row.imagesCount}</TableCell>
                  <TableCell align="right">{row.containCyrillic ? 'да' : 'нет'}</TableCell>
                  <TableCell align="right">{row.containNumbers ? 'да' : 'нет'}</TableCell>
                  <TableCell align="right">{row.containSpecialCharacters ? 'да' : 'нет'}</TableCell>
                  <TableCell align="right">{row.caseSensitivity ? 'да' : 'нет'}</TableCell>
                  <TableCell align="right">{row.answersType === 1 ? 'файл' : 'отсутсвует'}</TableCell>
                  <TableCell align="right">
                    <a href={`https://localhost:5001/DatasetUploader?fileName=${row.id}_${row.fileName}`} download>
                      $
                      {row.fileName}
                    </a>
                  </TableCell>
                </TableRow>
              )) : (<TableRow><TableCell align="right">empty</TableCell></TableRow>)}
            </TableBody>
          </Table>
        </TableContainer>
      </Container>
    </>
  )
}

export default UploadPage
