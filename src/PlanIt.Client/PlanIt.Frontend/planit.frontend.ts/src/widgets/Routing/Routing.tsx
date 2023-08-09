import React from 'react';
import { Route, Routes } from "react-router-dom";
import {
    LoginPage,
    PlanPage,
    ProfilePage,
} from "../../pages";
import { PrivateRoute } from "./PrivateRoute";
import { EnumUserRole } from "../../entities";
import { RegisterPage } from "../../pages";
import { LogoutPage } from "../../pages";

export const Routing = () => {
    return (
        <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="logout" element={<LogoutPage />} />
            <Route path="user/profile" element={<ProfilePage />} />
            <Route path="plans" element={
                <PrivateRoute requiredRole={EnumUserRole.user}>
                    <PlanPage />
                </PrivateRoute>
            } />
        </Routes >
    );
};