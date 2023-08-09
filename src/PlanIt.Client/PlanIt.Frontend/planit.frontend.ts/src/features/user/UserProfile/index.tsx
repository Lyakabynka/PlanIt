import React from 'react'
import { Grid } from '@mui/material';
import { useAuthStore } from '../../../entities';

export const UserProfile = () => {

    const { username, email, role, isEmailConfirmed } = useAuthStore();

    return (
        <Grid container direction="column" alignItems="center">
            <h4>Username: {username}</h4>
            <h6>Email: {email} | {isEmailConfirmed ? 'Email confirmed' : 'Confirm your email' }</h6>
            <h6>Role: {role}</h6>
        </Grid>
    )
}