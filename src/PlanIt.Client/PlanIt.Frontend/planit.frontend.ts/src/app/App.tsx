import React from 'react';
import { useEffect } from 'react'
import { Navbar, Routing } from "../widgets";
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import 'dayjs/locale/de';
import { useSignalRStore } from '../entities/sharedStores/useSignalRStore';
import { useAuthStore } from '../entities';

function App() {

  const theme = createTheme({
    typography: {
      fontFamily: 'Montserrat, sans-serif'
    },
    palette: {
      primary: {
        dark: '#ba323e', // black
        main: '#210700', // brown
        contrastText: '#fff5d7', // white
        light: '#f24b5a', // coral
      },
    },
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            backgroundColor: '#fff5d7',
          }
        }
      }
    }
  })

  const { establishConnection, closeConnection } = useSignalRStore();
  const { id, isLoggedIn } = useAuthStore();

  useEffect(() => {
    if (isLoggedIn && id !== null && isLoggedIn == true)
      establishConnection(id);
    else if (isLoggedIn)
      closeConnection();

  }, [isLoggedIn, id])


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
