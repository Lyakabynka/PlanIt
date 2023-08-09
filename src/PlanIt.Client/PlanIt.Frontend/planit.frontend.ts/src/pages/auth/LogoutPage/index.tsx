import React from 'react';
import {useAuthStore} from "../../../entities";
import { Navigate } from 'react-router-dom';

export const LogoutPage = () => {

    const {isLoggedIn, logout} = useAuthStore();
    
    if (isLoggedIn) logout().then();
    
    return <Navigate to='/login'/>
};
