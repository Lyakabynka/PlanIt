import React, { useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import { AgGridReact } from 'ag-grid-react';
import { ColDef, RowDoubleClickedEvent, RowDragEndEvent, RowDragEvent, DragStoppedEvent, GridReadyEvent, GridApi } from 'ag-grid-community';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { usePlanStore } from '../../plan/usePlanStore';
import { IPlan, IPlanPlanGroup, ISetPlanToPlanGroupRequestModel } from '../../../entities';
import { $api, ENDPOINTS } from '../../../shared';
import { usePlanGroupStore } from '../usePlanGroupStore';
import { IPlanGroupFull } from '../../../entities/models/planGroup/planGroupFull';
import { Box, Typography } from '@mui/material';
import { ManagePlanGroupSaveButtonPlaceHolder } from '../../../features';
import WestIcon from '@mui/icons-material/West';
import './index.css'

export const ManagePlanGroupPage = () => {

    const { id } = useParams<{ id: string }>();

    const navigate = useNavigate();

    if (Object.getPrototypeOf(id) === undefined) {
        navigate('/plan-groups');
    }

    const { getPlans, plans } = usePlanStore();

    const [planGroup, setPlanGroup] = useState<IPlanGroupFull | null>(null);

    const { setPlansToPlanGroup } = usePlanGroupStore();


    const [planPlanGroupGridApi, setPlanPlanGroupGridApi] = useState<GridApi<IPlanPlanGroup>>();

    const onPlanPlanGroupGridReady = (e: GridReadyEvent<IPlanPlanGroup, any>) => {
        setPlanPlanGroupGridApi(e.api);
    }

    //fetching all plans
    useEffect(() => {
        getPlans();
    }, [])

    //fetching all plans in specific plangroup
    useEffect(() => {
        $api.get(ENDPOINTS.PLANGROUP.GET_PLANGROUP.replace(`{id:guid}`, String(id)))
            .then((response) => {

                if (response.status !== 200) {
                    navigate('/');
                }

                console.log(response.data);

                setPlanGroup(response.data);
            });
    }, []);

    const handlePlansRowDoubleClicked = (e: RowDoubleClickedEvent<IPlan, any>) => {
        setPlanGroup({
            ...planGroup!, planPlanGroups: [...planGroup!.planPlanGroups, {
                id: "",
                plan: {
                    id: e.data!.id,
                    name: e.data!.name,
                    information: e.data!.information,
                    type: e.data!.type,
                    executionPath: e.data!.executionPath
                }
            }]
        })
    };

    const handlePlanPlanGroupsDragStopped = (e: DragStoppedEvent<IPlanPlanGroup, any>) => {

        let planPlanGroups: IPlanPlanGroup[] = e.api.getRenderedNodes()
            .map<IPlanPlanGroup>((node) => ({
                id: node.data!.id,
                plan: {
                    id: node.data!.plan.id,
                    name: node.data!.plan.name,
                    information: node.data!.plan.information,
                    type: node.data!.plan.type,
                    executionPath: node.data!.plan.executionPath
                }
            }));
        console.log(planPlanGroups);


        setPlanGroup({ ...planGroup!, planPlanGroups: planPlanGroups });
    }

    const handlePlanPlanGroupsDoubleClicked = (e: RowDoubleClickedEvent<IPlanPlanGroup, any>) => {
        setPlanGroup({ ...planGroup!, planPlanGroups: planGroup!.planPlanGroups.filter(pg => {
            console.log(pg.id);
            console.log(e.node.data?.id);
            
            return pg.id != e.node.data!.id
        })})
    }

    const plansInGroupColumnDefs: ColDef<IPlanPlanGroup>[] = [
        { headerName: 'Index', valueGetter: 'node.rowIndex + 1', flex: 2, rowDrag: true },
        { headerName: 'Name', field: 'plan.name', flex: 4 },
        { headerName: 'Type', field: 'plan.type', flex: 4 },
        { headerName: 'Information', field: 'plan.information', flex: 4 },
        { headerName: 'Execution Path', field: 'plan.executionPath', flex: 4 },
    ];

    const planColumnDefs: ColDef<IPlan>[] = [
        { field: 'name', flex: 4 },
        { field: 'information', flex: 4 },
        { field: 'type', flex: 4 },
        { field: 'executionPath', flex: 4 }
    ];

    const defaultColDef = {
    }

    const handleClick = () => {
        const setPlanToPlanGroupRequestModels = planPlanGroupGridApi?.getRenderedNodes()
            .map<ISetPlanToPlanGroupRequestModel>(node => ({
                index: node.rowIndex!,
                planId: node.data!.plan.id,
            }))

        if (setPlanToPlanGroupRequestModels === undefined
            || setPlanToPlanGroupRequestModels === null
            || setPlanToPlanGroupRequestModels.length === 0)
            return;

        setPlansToPlanGroup(String(id), setPlanToPlanGroupRequestModels!);
    }

    return (
        <>
            <div
                style={{
                    display: 'flex',
                    justifyContent: 'center',
                    marginBottom: '-25px',
                    marginTop: '20px'
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
                    {planGroup?.name}
                </Typography>
            </div>
            <div style={{
                width: '100%',
                height: '75vh',
                display: 'inline-flex',
                paddingLeft: 40,
                paddingRight: 40,
                gap: '5%'
            }}>
                <div className='ag-theme-alpine-dark custom-theme-group' style={{ width: '47.5%' }}>
                    <Box sx={{
                        textAlign: 'center',
                        marginBottom: '10px'
                    }}>
                        <Typography sx={{
                            textAlign: "center",
                            color: "primary.contrastText",
                            fontSize: 35,
                            fontWeight: 600,
                            letterSpacing: '.1rem',
                            textDecoration: 'none',
                            caretColor: 'transparent',
                            padding: '5px 40px 5px 40px',
                            backgroundColor: 'primary.main',
                            display: 'inline-block',
                            borderRadius: '23px',
                        }}>
                            Group
                        </Typography>
                    </Box>
                    <AgGridReact
                        columnDefs={plansInGroupColumnDefs}
                        rowData={planGroup?.planPlanGroups}
                        rowDragManaged={true}
                        animateRows={true}
                        defaultColDef={defaultColDef}
                        onRowDoubleClicked={handlePlanPlanGroupsDoubleClicked}
                        onDragStopped={handlePlanPlanGroupsDragStopped}
                        onGridReady={onPlanPlanGroupGridReady}
                    />
                </div>
                {/* <Box sx={{
                    verticalAlign: 'center',
                    backgroundColor: 'primary.main',
                    mt: '20%',
                    borderRadius: 25,
                    display: 'inline-block',
                    maxWidth: '100%',
                    maxHeight: '100%',
                    overflow: 'hidden'
                }}>
                    <WestIcon/>
                </Box> */}
                <div className='ag-theme-alpine custom-theme-plans' style={{ width: '47.5%' }}>
                    <Box sx={{
                        textAlign: 'center',
                        marginBottom: '10px'
                    }}>
                        <Typography sx={{
                            textAlign: "center",
                            color: "primary.contrastText",
                            fontSize: 35,
                            fontWeight: 600,
                            letterSpacing: '.1rem',
                            textDecoration: 'none',
                            caretColor: 'transparent',
                            padding: '5px 40px 5px 40px',
                            backgroundColor: 'primary.main',
                            display: 'inline-block',
                            borderRadius: '23px',
                        }}>
                            Plans
                        </Typography>
                    </Box>
                    <AgGridReact
                        columnDefs={planColumnDefs}
                        rowData={plans}
                        rowDragManaged={true}
                        animateRows={true}
                        defaultColDef={defaultColDef}
                        onRowDoubleClicked={handlePlansRowDoubleClicked}
                    />
                </div>
            </div>
            <ManagePlanGroupSaveButtonPlaceHolder handle={handleClick} />
        </>
    )
}
