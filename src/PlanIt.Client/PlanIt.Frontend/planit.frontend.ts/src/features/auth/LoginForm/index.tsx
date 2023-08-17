import {
    Avatar,
    Box,
    Button,
    CircularProgress,
    Container,
    Grid,
    TextField,
    Typography
} from "@mui/material";
import React, { useEffect } from "react";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { useAuthStore } from "../../../entities";
import { useFormik } from 'formik'; // Import Formik library
import * as Yup from 'yup'; // Import Yup for validation
import Link from '@mui/material/Link';

export function LoginForm() {

    const { login, resetErrorInfo, isLoading, errorMessage } = useAuthStore();

    const LOGIN_SCHEMA = Yup.object().shape({
        username: Yup.string().min(3, 'Username is too short').max(20, 'Username is too long').required('Username is required'),
        password: Yup.string().min(4, 'Password is too short').max(30, 'Password is too long').required('Password is required')
    });

    const formik = useFormik({
        initialValues: {
            username: '',
            email: '',
            password: '',
        },
        validationSchema: LOGIN_SCHEMA,
        onSubmit: (values) => {
            const usernameStr = values.username;
            const passwordStr = values.password;

            login({ userName: usernameStr, password: passwordStr });

            console.log(values);
        },
    });

    useEffect(() => {
        formik.validateForm();
        resetErrorInfo();
    }, []);

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
                    Authorize
                </Typography>
                <Box component="form" noValidate sx={{ mt: 3 }}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <TextField
                                required
                                fullWidth
                                autoComplete="username"
                                name="username"
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
                        {/* <Grid item xs={12}>
                            <FormControlLabel
                                control={<Checkbox value="allowExtraEmails" color="primary" />}
                                label="I want to receive inspiration, marketing promotions and updates via email."
                            />
                        </Grid> */}
                    </Grid>
                    <Button
                        type="submit"
                        onClick={formik.submitForm}
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                    >
                        Sign in
                    </Button>
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Link href="#" variant="body2" sx={{ textDecoration: 'none' }}>
                                Already have an account? Sign in
                            </Link>
                        </Grid>
                    </Grid>
                    {
                        isLoading &&
                        <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                            <CircularProgress />
                        </Box>
                    }
                </Box>
            </Box>
        </Container>
    );
}