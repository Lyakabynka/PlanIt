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
import { PlanCardDropDown } from './PlanCardDropDown';

interface IPlanProps {
    plan: IPlan
}

export const PlanCard: React.FC<IPlanProps> = ({ plan }) => {

    const [openSchedulePlanDialog, setOpenSchedulePlanDialog] = React.useState<boolean>(false);

    const handleOpenSchedulePlanDialog = () => {
        setOpenSchedulePlanDialog(true);
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
                id={plan.id}>
                <CardContent>
                    <PlanCardDropDown planId={plan.id} />
                    <Typography
                        variant="h6"
                        component="div"
                        sx={{
                            color: 'primary.contrastText',
                            textAlign: 'center',
                            verticalAlign: 'center',
                            height: 70,
                        }}>
                        {plan.name}
                    </Typography>
                    <Typography
                        sx={{
                            mb: 1.5,
                            color: 'primary.contrastText',
                            textAlign: 'center',
                            verticalAlign: 'center',
                            height: 70,
                            opacity: 0.8,
                            lineHeight: 'normal'
                        }}>
                        {plan.type}
                    </Typography>
                    <Typography
                        variant="body2"
                        flexWrap='wrap'
                        sx={{
                            color: 'primary.contrastText',
                            textAlign: 'center',
                            verticalAlign: 'center',
                            opacity: 0.9
                        }}>
                        {plan.information}
                    </Typography>
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
                        onClick={handleOpenSchedulePlanDialog}>
                        Schedule
                    </Button>
                </CardActions>
            </Card>
            <SchedulePlanDialog planId={plan.id} open={openSchedulePlanDialog} setOpen={setOpenSchedulePlanDialog} />
        </Box >
    )
}