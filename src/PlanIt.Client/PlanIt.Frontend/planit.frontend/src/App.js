import { Box, CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import './App.css';
import Header from './components/Header/Header';
import { Route, Routes } from 'react-router-dom';
import SignUp from './components/Auth/SignUp';
import { PagesProvider } from './components/context/PagesContext';

function App() {

  const theme = createTheme({
    palette: {
      primary: {
        dark: '#c9bb91',
        main: '#fff5d7',
        contrastText: '#210700'
      },
    },
  })

  

  return (
    <>
      <CssBaseline />
      <ThemeProvider theme={theme}>
        <PagesProvider>
          <Header />
          <Routes>
            <Route path='/signup' element={<SignUp />} />
          </Routes>
        </PagesProvider>
      </ThemeProvider>
    </>
  );
}

export default App;
