import { AgGridReact } from 'ag-grid-react';
import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import { $api, ENDPOINTS } from '../../../shared';
import { ColDef, RowDoubleClickedEvent, RowDragEndEvent, RowDragEvent, DragStoppedEvent, GridReadyEvent, GridApi } from 'ag-grid-community';
import { EnumScheduledPlanType, IPlanSchedule, IScheduledPlan } from '../../../entities';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import './index.css'
import { Box, Typography } from '@mui/material';

export const ScheduledPlanPage = () => {

    const { id } = useParams<{ id: string }>();

    const navigate = useNavigate();

    const [planSchedule, setPlanSchedule] = useState<IPlanSchedule | null>(null)

    const getScheduledPlans = () => {
        $api.get(ENDPOINTS.SCHEDULED_PLAN.GET_SCHEDULED_PLANS.replace('{planId:guid}', String(id)))
            .then((response) => {
                if (response.status !== 200) {
                    navigate('/');
                }

                setPlanSchedule(response.data);
            });
    }

    useEffect(() => {
        getScheduledPlans();
    }, [])

    const scheduledPlanColumnDefs: ColDef<IScheduledPlan>[] = [
        { field: 'type', flex: 4, filter: true },
        {
            headerName: 'Schedule', valueGetter: (s) => {
                const scheduledPlan = s.data;
                switch (scheduledPlan?.type) {
                    case EnumScheduledPlanType.oneOff:
                        const date = new Date(scheduledPlan.executeUtc + 'Z');
                        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
                    case EnumScheduledPlanType.recurring:
                        return `UTC: ${scheduledPlan.cronExpressionUtc}`;
                    default:
                        return 'Undefined';
                }
            },
            flex: 4,
            filter: true
        }
    ];

    const handleScheduledPlansDoubleClicked = (e: RowDoubleClickedEvent<IScheduledPlan>) => {
        let id = e.data?.id;
        console.log(id);
        
        if (id !== undefined || id !== null) {
            id = String(id);
            $api.delete(ENDPOINTS.SCHEDULED_PLAN.DELETE_SCHEDULED_PLAN
                .replace('{id:guid}', id))
                .then(response => {
                    if (response.status === 200) {
                        setPlanSchedule({
                            ...planSchedule!, scheduledPlans: planSchedule!.scheduledPlans.filter(sp => sp.id !== id)
                        });
                    }
                });
        }
    }

    const defaultColDef = {
    }

    return (
        <>
            <div
                style={{
                    display: 'flex',
                    justifyContent: 'center',
                    marginBottom: '25px',
                    marginTop: '40px'
                }}>
                <Typography sx={{
                    textAlign: "center",
                    color: "primary.main",
                    fontSize: 38,
                    fontWeight: 600,
                    letterSpacing: '.05rem',
                    textDecoration: 'none',
                    caretColor: 'transparent',
                    padding: '3px 20px 3px 20px',
                    backgroundColor: 'rgba(255,245,215)',
                    display: 'inline-block',
                    borderRadius: '23px',
                }}>
                    {planSchedule?.name}
                </Typography>
            </div>
            <div style={{
                display: 'flex',
                justifyContent: 'center',
            }}>
                <div style={{
                    width: '50%',
                    height: '75vh',
                    display: 'inline-flex',
                }}>
                    <div className='ag-theme-alpine-dark custom-theme-group'
                        style={{
                            width: '100%'
                        }}>
                        <AgGridReact
                            columnDefs={scheduledPlanColumnDefs}
                            rowData={planSchedule?.scheduledPlans}
                            defaultColDef={defaultColDef}
                            onRowDoubleClicked={handleScheduledPlansDoubleClicked}
                        />
                    </div>
                </div>
            </div>
        </>
    )
}
