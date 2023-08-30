import { Box, Card, CardContent, Typography, Button, CardActions } from '@mui/material'
import React from 'react'
import { IPlanGroup } from '../../../entities/models/planGroup/planGroup'
import { usePlanStore } from '../../../pages/plan/usePlanStore'

interface IPlanGroupProps {
    planGroup: IPlanGroup
}

export const PlanGroupCard: React.FC<IPlanGroupProps> = ({ planGroup }) => {

    

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
                        onClick={}
                    >
                        Schedule
                    </Button>
                </CardActions>
            </Card>

        </Box >
    )
}
