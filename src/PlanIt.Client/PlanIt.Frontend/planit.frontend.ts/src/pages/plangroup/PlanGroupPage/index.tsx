import React, { useEffect } from 'react'
import { usePlanGroupStore } from '../usePlanGroupStore'
import { Box, CircularProgress } from '@mui/material';
import { PlanGroupCard } from '../../../features';
import { IPlanGroup } from '../../../entities/models/planGroup/planGroup';
import { CreatePlanGroupListenPlaceHolder } from '../../../features/plangroup/CreatePlanGroupListenPlaceHolder';

export const PlanGroupPage = () => {

    const { isLoading, planGroups, getPlanGroups } = usePlanGroupStore();

    useEffect(() => {
        getPlanGroups();
    }, [])

    return (
        <>
            {isLoading ?
                <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                    <CircularProgress />
                </Box>
                :
                <Box>
                    {planGroups?.map((planGroup: IPlanGroup) => {
                        return <PlanGroupCard key={planGroup.id} planGroup={planGroup} />
                    })}
                </Box>
            }
            <CreatePlanGroupListenPlaceHolder/>
        </>
    )
}
