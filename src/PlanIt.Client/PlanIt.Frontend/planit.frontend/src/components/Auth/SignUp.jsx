import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { useFormik } from 'formik'; // Import Formik library
import * as Yup from 'yup'; // Import Yup for validation

export default function SignUp() {

  const handleSubmit = (event) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);

    console.log({
      username: data.get('username'),
      email: data.get('email'),
      password: data.get('password'),
    });
  };

  const PRODUCT_SCHEMA = Yup.object().shape({
    username: Yup.string().min(3, 'Username is too short').max(20, 'Username is too long').required('Username is required.'),
    email: Yup.string().email('Email must be valid').required('Email is required'),
    password: Yup.string().min(4, 'Password is too short').max(30, 'Password is too long').required('Password is required')
  });

  const formik = useFormik({
    initialValues: {
      username: '',
      email: '',
      password: '',
    },
    validationSchema: PRODUCT_SCHEMA,
    onSubmit: (values) => {
      // Handle form submission here
      console.log(values);
    },
  });

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
                onLoad={formik.validateForm}
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
            <Grid item xs={12}>
              <FormControlLabel
                control={<Checkbox value="allowExtraEmails" color="primary" />}
                label="I want to receive inspiration, marketing promotions and updates via email."
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
              <Link href="#" variant="body2">
                Already have an account? Sign in
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Box>
    </Container>
  );
}