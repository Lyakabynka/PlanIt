import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Typography from '@mui/material/Typography';
import { IPlan } from '../../../entities'
import Button from '@mui/material/Button';

import { SchedulePlanDialog } from '../SchedulePlanDialog';
import { usePlanStore } from '../../../pages/plan/usePlanStore';
import { Grid } from '@mui/material';

interface IPlanProps {
    plan: IPlan
}

export const PlanCard: React.FC<IPlanProps> = ({ plan }) => {
    
    const [open, setOpen] = React.useState(false);

    const { deletePlan } = usePlanStore();

    const handleDeletePlan = () => {
        deletePlan(plan.id).then();
    }

    const handleSchedulePlan = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <Box sx={{
            display: 'inline-flex',
            margin: '15px',
            caretColor: 'transparent',
        }}>
            <Card
                sx={{
                    backgroundColor: 'primary.light',
                    minWidth: 200,
                    maxWidth: 200,
                    color: 'primary.dark',
                }}
                id={plan.id}>
                <CardContent>
                    <Typography variant="h6" component="div">
                        {plan.name}
                    </Typography>
                    <Typography sx={{ mb: 1.5 }}>
                        {plan.type}
                    </Typography>
                    <Typography variant="body2" flexWrap='wrap'>
                        {plan.information}
                    </Typography>
                </CardContent>
                <CardActions>
                        <Grid
                            xs={12}
                            container
                            rowSpacing={1}
                            columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
                            <Grid item xs={4} md={4}>
                                <Button size="small" sx={{
                                    color: 'primary.dark',
                                    textTransform: 'none',
                                    textAlign: 'center'
                                }} onClick={handleSchedulePlan}>
                                    Schedule
                                </Button>
                            </Grid>

                            <Grid item xs={4} md={4}>
                                <Button size="small" sx={{
                                    color: 'primary.dark',
                                    textTransform: 'none',
                                    textAlign: 'center'
                                }}>
                                    Edit
                                </Button>
                            </Grid>

                            <Grid item xs={4} md={4}>
                                <Button size="small" sx={{
                                    color: 'primary.dark',
                                    textTransform: 'none',
                                    textAlign: 'center'
                                }} onClick={handleDeletePlan}>
                                    Delete
                                </Button>
                            </Grid>
                        </Grid>
                </CardActions>
            </Card>
            <SchedulePlanDialog planId={plan.id} open={open} setOpen={setOpen} />
        </Box >
    )
}