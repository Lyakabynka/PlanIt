import { Box, Card, CardContent, Typography, Button, CardActions, IconButton } from '@mui/material'
import React from 'react'
import { IPlanGroup } from '../../../entities/models/planGroup/planGroup'
import { PlanGroupCardDropDown } from './PlanGroupCardDropDown'
import AddIcon from '@mui/icons-material/Add';
import { useNavigate } from 'react-router-dom';
import { SchedulePlanGroupDialog } from '../SchedulePlanGroupDialog';

interface IPlanGroupProps {
    planGroup: IPlanGroup
}

export const PlanGroupCard: React.FC<IPlanGroupProps> = ({ planGroup }) => {

    const navigate = useNavigate();

    const [openSchedulePlanGroupDialog, setOpenSchedulePlanGroupDialog] = React.useState<boolean>(false);

    const handleOpenSchedulePlanGroupDialog = () => {
        setOpenSchedulePlanGroupDialog(true);
    }

    const handleOpenAddPlanToPlanGroup = () => {
        navigate(`/plan-groups/${planGroup.id}`);
    }

    return (
        <Box sx={{
            display: 'inline-flex',
            margin: '15px',
            caretColor: 'transparent',
        }}>
            <Card
                sx={{
                    backgroundColor: 'primary.light',
                    minWidth: 230,
                    maxWidth: 230,
                    minHeight: 290,
                    maxHeight: 290,
                    color: 'primary.dark',
                    borderRadius: 3,
                    position: 'relative',
                }}
                id={planGroup.id}>
                <CardContent>
                    <PlanGroupCardDropDown planGroupId={planGroup.id} />
                    <Typography
                        variant="h6"
                        component="div"
                        sx={{
                            color: 'primary.contrastText',
                            textAlign: 'center',
                            verticalAlign: 'center',
                            height: 70,
                        }}>
                        {planGroup.name}
                    </Typography>
                    <Typography
                        variant="h6"
                        component="div"
                        sx={{
                            color: 'primary.contrastText',
                            textAlign: 'center',
                            verticalAlign: 'center',
                            height: 30,
                        }}>
                        Plans: {planGroup.planCount}
                    </Typography>
                    <IconButton sx={{

                    }} onClick={handleOpenAddPlanToPlanGroup}>
                        <AddIcon />
                    </IconButton>
                </CardContent>
                <CardActions sx={{
                    padding: 0,
                }}>
                    <Button
                        size="small"
                        sx={{
                            color: 'primary.contrastText',
                            textTransform: 'none',
                            textAlign: 'center',

                            width: '100%',
                            backgroundColor: 'primary.dark',
                            borderRadius: 0,
                            bottom: 0,
                            position: 'absolute',

                            borderBottomLeftRadius: 12,
                            borderBottomRightRadius: 12,
                        }}
                        onClick={handleOpenSchedulePlanGroupDialog}
                    >
                        Schedule
                    </Button>
                </CardActions>
            </Card>
            <SchedulePlanGroupDialog
                open={openSchedulePlanGroupDialog}
                setOpen={setOpenSchedulePlanGroupDialog}
                planGroupId={planGroup.id} />
        </Box >
    )
}
