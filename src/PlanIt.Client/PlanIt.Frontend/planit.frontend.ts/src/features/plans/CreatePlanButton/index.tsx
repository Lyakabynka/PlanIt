import React from 'react'
import { Button, IconButton } from '@mui/material'
import AddIcon from '@mui/icons-material/Add';
import { CreatePlanDialog } from '../CreatePlanDialog';

export const CreatePlanButton = () => {

    const [addPlanDialogOpen, setAddPlanDialogOpen] = React.useState<boolean>(false);

    const handleCreatePlanDialogOpen = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        setAddPlanDialogOpen(true);
    }

    return (
        <>
            <IconButton
                size='large'
                sx={{
                    backgroundColor: 'primary.main',
                    color: 'primary.contrastText',
                    '&:hover': { backgroundColor: 'primary.dark' },
                    marginRight: 2
                }}
                onClick={handleCreatePlanDialogOpen}>
                <AddIcon />
            </IconButton>
            <CreatePlanDialog open={addPlanDialogOpen} setOpen={setAddPlanDialogOpen} />
        </>
    )
}
