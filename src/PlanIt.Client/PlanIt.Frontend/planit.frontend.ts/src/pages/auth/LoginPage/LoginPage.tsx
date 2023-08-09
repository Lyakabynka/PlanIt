import React from 'react';
import {useAuthStore} from "../../../entities";
import {Navigate} from "react-router-dom";
import {EnumUserRole} from "../../../entities";
import {LoginForm} from "../../../features";


export const LoginPage = () => {

    const {role, isLoggedIn} = useAuthStore();
    console.log(role);
    
    if (isLoggedIn) {
        switch (role) {
            case EnumUserRole.user:
                return <Navigate to='/user'/>
            case EnumUserRole.administrator:
                return <Navigate to='/administrator'/>
            default:
                console.error('Unexpected user role');
                break;
        }
    }

    return (
        <LoginForm/>
    );
};