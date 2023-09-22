import React from 'react'
import CheckIcon from '@mui/icons-material/Check';
import { Box, IconButton } from '@mui/material';

interface IManagePlanGroupSaveButtonPlaceHolderProps {
    handle: () => void
}

export const ManagePlanGroupSaveButtonPlaceHolder: React.FC<IManagePlanGroupSaveButtonPlaceHolderProps> = ({ handle }) => {
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
            <IconButton
                onClick={handle}>
                <CheckIcon sx={{
                    color: 'primary.contrastText'
                }} />
            </IconButton>
            {/* <Listen/> */}
        </Box>
    )
}
