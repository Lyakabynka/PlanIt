import React from 'react';
import { Route, Routes } from "react-router-dom";
import {
    LoginPage,
    MainPage,
    PlanGroupPage,
    PlanPage,
    ProfilePage,
} from "../../pages";
import { PrivateRoute } from "./PrivateRoute";
import { EnumUserRole } from "../../entities";
import { RegisterPage } from "../../pages";
import { LogoutPage } from "../../pages";
import { ManagePlanGroupPage } from '../../pages';
import { ScheduledPlanPage } from '../../pages/plan/ScheduledPlanPage';

export const Routing = () => {
    return (
        <Routes>
            <Route path="/" element={<MainPage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="logout" element={<LogoutPage />} />
            <Route path="user/profile" element={<ProfilePage />} />
            <Route path="plans" element={
                <PrivateRoute requiredRole={EnumUserRole.user}>
                    <PlanPage />
                </PrivateRoute>
            } />
            <Route path="plan-groups" element={
                <PrivateRoute requiredRole={EnumUserRole.user}>
                    <PlanGroupPage />
                </PrivateRoute>
            } />
            <Route path="plan-groups/:id" element={
                <PrivateRoute requiredRole={EnumUserRole.user}>
                    <ManagePlanGroupPage />
                </PrivateRoute>
            } />

            <Route path="plans/:id/scheduled" element={
                <PrivateRoute requiredRole={EnumUserRole.user}>
                    <ScheduledPlanPage />
                </PrivateRoute>
            } />
        </Routes >
    );
};