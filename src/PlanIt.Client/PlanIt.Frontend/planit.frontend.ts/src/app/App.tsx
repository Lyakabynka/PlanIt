import React from 'react';
import { useEffect } from 'react'
import { Navbar, Routing } from "../widgets";
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import 'dayjs/locale/de';
import { useSignalRStore } from '../entities/index';
import { useAuthStore } from '../entities';
import './App.css'
import backgroundImage from '../background.svg';

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
            backgroundColor: '#fff5d7'
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
        <div style={{
          backgroundImage: `url(${backgroundImage})`,
          backgroundSize: 'cover', // or 'contain' depending on your preference
          backgroundRepeat: 'no-repeat',
          backgroundAttachment: 'fixed', // Optional: Makes the background image fixed
          minHeight: '100vh', // Ensures the background covers the entire viewport
          backgroundPosition: 'center top 0px',
          
        }}>
          <Navbar />
          <Routing />
        </div>
      </LocalizationProvider>
    </ThemeProvider>
  );
}

export default App;
