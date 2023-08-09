import React, { useEffect } from 'react';
import { EnumUserRole, useAuthStore } from "../../../entities";
import { Navigate } from "react-router-dom";
import { Box, CircularProgress } from "@mui/material";
import { UserProfile } from '../../../features';

export const ProfilePage = () => {

    const { isLoggedIn, role } = useAuthStore();

    if (!isLoggedIn)
        return <Navigate to='/login' />

    return (
        <UserProfile />
    )
};
