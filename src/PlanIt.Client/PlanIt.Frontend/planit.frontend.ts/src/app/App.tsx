import React from 'react';
import { Navbar, Routing } from "../widgets";
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import 'dayjs/locale/de';

function App() {

  const theme = createTheme({
    typography:{
      fontFamily: 'Montserrat, sans-serif'
    },
    palette: {
      primary: {
        dark: '#c9bb91',
        main: '#210700',
        contrastText: '#fff5d7',
        light: '#3b0c00'
      },
    },
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            backgroundColor: '#c9bb91'
          }
        }
      }
    }
  })

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale='de'>
        <Navbar />
        <Routing />
      </LocalizationProvider>
    </ThemeProvider>
  );
}

export default App;
