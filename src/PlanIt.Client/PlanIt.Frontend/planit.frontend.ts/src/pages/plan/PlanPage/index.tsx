import React, { useEffect } from "react";
import { usePlanStore } from '../usePlanStore'
import { IPlan, useAuthStore } from '../../../entities';
import { Box, CircularProgress, Container, Grid } from '@mui/material';
import { CreatePlanListenPlaceHolder, PlanCard } from "../../../features";


export const PlanPage = () => {

    const { isLoading, plans, getPlans } = usePlanStore();

    useEffect(() => {
        getPlans();
    }, [])

    return (
        <>
            {isLoading ?
                <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                    <CircularProgress />
                </Box>
                :
                <Box>
                    {plans?.map((plan: IPlan) => {
                        return <PlanCard key={plan.id} plan={plan} />
                    })}
                </Box>
            }
            <CreatePlanListenPlaceHolder />
        </>
    )
}