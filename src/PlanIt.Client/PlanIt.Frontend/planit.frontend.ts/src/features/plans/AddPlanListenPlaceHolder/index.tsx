import { Box } from '@mui/material'
import React from 'react'
import { AddPlanButton } from '../AddPlanButton'
import { Listen } from '../Listen'

export const AddPlanListenPlaceHolder = () => {
    return (
        <Box sx={{
            position: 'fixed',
            bottom: 10,
            left: "50%",
            transform: "translateX(-50%)",
            flexDirection: 'row',
            backgroundColor: 'primary.main',
            borderRadius: 25
        }}>
            <AddPlanButton />
            <Listen/>
        </Box>
    )
}
