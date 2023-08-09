import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import React, { useState } from 'react'
import { EnumPlanType } from '../../../entities'
import { usePlanStore } from '../../../pages/plan/usePlanStore'

interface AddPlanDialogProps {
    open: boolean,
    setOpen: (open: boolean) => void,

}

export const AddPlanDialog: React.FC<AddPlanDialogProps> = ({ open, setOpen }) => {

    const { addPlan } = usePlanStore();

    const planTypeValues = Object.values(EnumPlanType);

    const [type, setType] = useState('');

    const [name, setName] = useState<string>('');
    const [information, setInformation] = useState<string>('');
    const [executionPath, setExecutionPath] = useState<string | null>(null);

    const handleClose = () => {
        setOpen(false);
    };

    const handleSubmit = () => {
        addPlan({
            type: type,
            name: name,
            information: information,
            executionPath: executionPath
        }).then();

        setOpen(false);
    };

    const convertTypeToReadableType = (type: string) => {
        return type;
    }

    return (
        <Dialog
            open={open}
            // onClose={handleClose}
            maxWidth='xs'>
            <DialogTitle>Plan it!</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Add new plan to schedule it in the future!
                </DialogContentText>
                <FormControl

                    fullWidth
                    margin='normal'>
                    <InputLabel id="type-label">Type</InputLabel>
                    <Select
                        labelId="type-label"
                        id="type"
                        name='type'
                        label="Type"
                        value={type}
                        required
                        onChange={(e) => setType(e.target.value)}
                    >
                        {planTypeValues.map(type => {
                            return <MenuItem key={type} value={type}>{convertTypeToReadableType(type)}</MenuItem>
                        })}
                    </Select>
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
                    <TextField
                        sx={{ marginTop: '10px' }}
                        required
                        fullWidth
                        id="information"
                        label="Information"
                        name="information"
                        autoComplete="information"
                        value={information}
                        onChange={(e) => setInformation(e.target.value)}
                    />
                    <TextField
                        sx={{ marginTop: '10px' }}
                        fullWidth
                        id="execution-path"
                        label="Execution Path"
                        name="execution-path"
                        autoComplete="execution-path"
                        value={executionPath}
                        onChange={(e) => setExecutionPath(e.target.value)}
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
