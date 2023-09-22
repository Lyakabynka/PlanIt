import { Box } from '@mui/material'
import React from 'react'
import { CreatePlanGroupButton } from '../CreatePlanGroupButton'

export const CreatePlanGroupListenPlaceHolder = () => {
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
        <CreatePlanGroupButton />
        {/* <Listen/> */}
    </Box>
  )
}
