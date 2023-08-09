import React, {useEffect} from 'react';
import {
    Avatar,
    Box,
    Button,
    Checkbox,
    CircularProgress,
    Container,
    FormControlLabel,
    TextField,
    Typography
} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import { useFormik } from 'formik'; // Import Formik library
import * as Yup from 'yup'; // Import Yup for validation
import {useAuthStore} from "../../../entities";
import { Link, useNavigate } from 'react-router-dom';
import Grid from '@mui/material/Grid';

export const RegisterForm = () => {

    const {register, resetErrorInfo, isLoading, errorMessage} = useAuthStore();
    const navigate = useNavigate();

    const REGISTER_SCHEMA = Yup.object().shape({
        username: Yup.string().min(3, 'Username is too short').max(20, 'Username is too long').required('Username is required'),
        email: Yup.string().max(60, 'Email is too long').required('Email is required')
            .test('email-match', 'Email is not valid', function (value) {
                const emailRegex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
                return emailRegex.test(value);
            }),
        password: Yup.string().min(4, 'Password is too short').max(30, 'Password is too long').required('Password is required')
    });

    const formik = useFormik({
        initialValues: {
          username: '',
          email: '',
          password: '',
        },
        validationSchema: REGISTER_SCHEMA,
        onSubmit: (values) => {
          // Handle form submission here
          console.log(values);
        },
      });

    useEffect(() => {
        resetErrorInfo();
        formik.validateForm();
    }, []);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);
        
        if (data.get('username') && data.get('password') && data.get('email')) {
            const usernameStr = data.get('username')!.toString();
            const passwordStr = data.get('password')!.toString();
            const emailStr = data.get('email')!.toString();

            console.log(emailStr);
            
            await register({username: usernameStr, email: emailStr, password: passwordStr});

            if(errorMessage?.length == 0)
                navigate('/login');
        }
    };

    return (
        <Container component="main" maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
        }}
      >
        <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          Sign up
        </Typography>
        <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={12}>
              <TextField
                autoComplete="username"
                name="username"
                fullWidth
                id="username"
                label="Username"
                autoFocus
                value={formik.values.username}
                onChange={formik.handleChange}
                error={Boolean(formik.errors.username)}
                helperText={formik.errors.username}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                required
                fullWidth
                id="email"
                label="Email Address"
                name="email"
                autoComplete="email"
                value={formik.values.email}
                onChange={formik.handleChange}
                error={Boolean(formik.errors.email)}
                helperText={formik.errors.email}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                required
                fullWidth
                name="password"
                label="Password"
                type="password"
                id="password"
                autoComplete="new-password"
                value={formik.values.password}
                onChange={formik.handleChange}
                error={Boolean(formik.errors.password)}
                helperText={formik.errors.password}
              />
            </Grid>
          </Grid>
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Sign Up
          </Button>
          <Grid container justifyContent="flex-end">
            <Grid item>
              <Link to="/login" style={{textDecoration: 'none', color: 'inherit'}}>
                Already have an account? Sign in
              </Link>
            </Grid>
          </Grid>
          {
            isLoading &&
                <Box sx={{display: 'flex', justifyContent: 'center'}}>
                    <CircularProgress/>
                </Box>
          }
        </Box>
      </Box>
    </Container>
    );
};
