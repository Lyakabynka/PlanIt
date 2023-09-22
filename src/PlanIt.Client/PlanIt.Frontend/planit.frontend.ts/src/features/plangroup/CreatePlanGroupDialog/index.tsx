import React, { useState } from 'react'
import { usePlanGroupStore } from '../../../pages/plangroup/usePlanGroupStore';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'

interface CreatePlanGroupDialogProps {
    open: boolean,
    setOpen: (open: boolean) => void,

}

export const CreatePlanGroupDialog: React.FC<CreatePlanGroupDialogProps> = ({ open, setOpen }) => {
    const { createPlanGroup } = usePlanGroupStore();

    const [name, setName] = useState<string>('');

    const handleClose = () => {
        setOpen(false);
    };

    const handleSubmit = () => {
        createPlanGroup({
            name: name,
        });

        setOpen(false);
    };

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            maxWidth='xs'>
            <DialogTitle>Make a group</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Add a new group of plans
                </DialogContentText>
                <FormControl
                    fullWidth
                    margin='normal'>
                    <TextField
                        sx={{ marginTop: '10px' }}
                        required
                        fullWidth
                        id="name"
                        label="Name"
                        name="name"
                        autoComplete="name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <Button type='submit' onClick={handleSubmit}>Submit</Button>
            </DialogActions>
        </Dialog>
    )
}
