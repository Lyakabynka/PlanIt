import React, { useEffect } from "react";
import { usePlanStore } from '../usePlanStore'
import { IPlan, useAuthStore } from '../../../entities';
import { Box, CircularProgress } from '@mui/material';
import { PlanCard } from "../../../features";
import { PlanAppBar } from "../../../features/plans/PlanAppBar";

export const PlanPage = () => {

    const { isLoading, plans, getPlans } = usePlanStore();

    useEffect(() => {
        getPlans();
    }, [])

    return (
        <>
            <PlanAppBar/>
            {isLoading ?
                <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                    <CircularProgress />
                </Box>
                :
                plans?.map((plan: IPlan) => {
                    return <PlanCard key={plan.id} plan={plan} />
                })
            }
        </>
    )
}