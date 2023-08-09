import React from 'react';
import { EnumUserRole, useAuthStore } from "../../../entities";
import { Navigate, useNavigate } from "react-router-dom";
import { RegisterForm } from "../../../features";
import path from 'path';

export const RegisterPage = () => {

    const { role, isLoggedIn } = useAuthStore();

    if (isLoggedIn) {
        switch (role) {
            case EnumUserRole.user:
                return <Navigate to="/user" />
            case EnumUserRole.administrator:
                return <Navigate to='/administrator' />
            default:
                console.error('Unexpected user role');
                break;
        }
    }

    return (<RegisterForm />);
};
