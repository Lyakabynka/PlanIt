import React from 'react'
import { Button, IconButton } from '@mui/material'
import AddIcon from '@mui/icons-material/Add';
import { AddPlanDialog } from '../AddPlanDialog';

export const AddPlanButton = () => {

    const [addPlanDialogOpen, setAddPlanDialogOpen] = React.useState<boolean>(false);

    const handleAddPlanDialogOpen = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        setAddPlanDialogOpen(true);
    }

    return (
        <>
            <IconButton
                size='large'
                sx={{
                    position: 'fixed',
                    bottom: 10,
                    left: "50%",
                    transform: "translateX(-50%)",
                    backgroundColor: 'primary.main',
                    color: 'primary.contrastText',
                    '&:hover': { backgroundColor: 'primary.dark' },
                }}
                onClick={handleAddPlanDialogOpen}>
                <AddIcon />
            </IconButton>
            <AddPlanDialog open={addPlanDialogOpen} setOpen={setAddPlanDialogOpen} />
        </>
    )
}
