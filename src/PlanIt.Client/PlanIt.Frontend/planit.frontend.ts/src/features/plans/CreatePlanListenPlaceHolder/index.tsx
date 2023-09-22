import { Box } from '@mui/material'
import React from 'react'
import { CreatePlanButton } from '../CreatePlanButton'
import { Listen } from '../Listen'

export const CreatePlanListenPlaceHolder = () => {
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
            <CreatePlanButton />
            <Listen/>
        </Box>
    )
}
