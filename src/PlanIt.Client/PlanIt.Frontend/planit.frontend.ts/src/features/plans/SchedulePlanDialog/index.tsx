import React, { CSSProperties, useState } from 'react'
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import MenuItem from '@mui/material/MenuItem';
import { DateTimePicker, DatePicker } from '@mui/x-date-pickers';
import { useFormik } from 'formik';
import { FormControl, InputLabel, Select, SelectChangeEvent, Button } from '@mui/material';
import dayjs, { Dayjs } from 'dayjs'
import * as Yup from 'yup'; // Import Yup for validation
import { usePlanStore } from '../../../pages/plan/usePlanStore';
import { EnumScheduledPlanType } from '../../../entities';
import Cron from 'react-js-cron';
import '../SchedulePlanDialog/index.css'

interface SchedulePlanDialogProps {
    planId: string,
    open: boolean,
    setOpen: (params: boolean) => void,
}

export const SchedulePlanDialog: React.FC<SchedulePlanDialogProps> =
    ({ planId, open, setOpen }) => {

        const { schedulePlan } = usePlanStore();

        const [type, setType] = useState('');
        const [dateTime, setDateTime] = useState<Dayjs>(dayjs());
        const [cronExpression, setCronExpression] = useState('');

        const handleClose = () => {
            setOpen(false);
        };

        const showInputField = () => {
            switch (type) {
                case "Instant":
                    break;
                case "OneOff":
                    return <DateTimePicker
                        sx={{ marginTop: '10px' }}
                        label="Select date"
                        onChange={(e) => setDateTime(dayjs(e))}
                        value={dateTime}
                    />
                case "Recurring":
                    return <Cron
                        value={cronExpression}
                        setValue={setCronExpression}
                        className='cron'
                    />
                // return <TextField
                //     sx={{ marginTop: '10px' }}
                //     required
                //     fullWidth
                //     name="cronExpression"
                //     label="Cron Epxression"
                //     type="text"
                //     id="cronExpression"
                //     onChange={(e) => setCronExpression(e.target.value)}
                //     value={cronExpression}
                // />
            }
        }

        const handleSubmit = (e: React.MouseEvent<HTMLButtonElement>) => {

            e.preventDefault();

            if (type === null) {
                return;
            }

            switch (type) {
                case "Instant":
                    setCronExpression(null!);
                    break;
                case "OneOff":
                    setCronExpression(null!);

                    if (dateTime === null) return;

                    setDateTime(dateTime.subtract(
                        dayjs().utcOffset(),
                        'minute'));
                    break;
                case "Recurring":
                    setDateTime(null!);

                    if (cronExpression === '') return;
                    break;
            }

            schedulePlan(planId, {
                type: type,
                executeUtc: dateTime?.toISOString(),
                cronExpressionUtc: cronExpression,
            }).then();

            handleClose();
        }

        return (
            <Dialog
                open={open}
                onClose={handleClose}
                maxWidth='xs'>
                <DialogTitle>Schedule</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Type in the required information
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
                            {Object.values(EnumScheduledPlanType).map((type) => {
                                return <MenuItem key={type} value={type}>{type}</MenuItem>
                            })}
                        </Select>
                        {showInputField()}
                    </FormControl>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button type='submit' onClick={handleSubmit}>Submit</Button>
                </DialogActions>
            </Dialog>
        )
    }
