import Box from "@mui/material/Box";
import IconButton from "@mui/material/IconButton";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import React from 'react'
import { PlanCardDropDownIcon } from "../PlanCardDropDownIcon";
import { usePlanStore } from "../../../../pages/plan/usePlanStore";

interface PlanCardDropDownProps {
    planId: string;
}

export const PlanCardDropDown: React.FC<PlanCardDropDownProps> = ({ planId }) => {

    //
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

    const handleOpen = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    //
    const { deletePlan } = usePlanStore();

    const handleDeletePlan = () => {
        deletePlan(planId).then();
    }
    //

    return (
        <Box sx={{ flexGrow: 1, position: 'relative', left: '90%' }}>
            <IconButton
                id="menu-button-card-actions"
                size="small"
                aria-label="menu-button-card-actions"
                aria-controls="menu-card-actions"
                aria-haspopup="true"
                onClick={handleOpen}
                color="inherit"
            >
                <PlanCardDropDownIcon />
            </IconButton>
            <Menu
                id="menu-card-actions"
                anchorEl={anchorEl}
                anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'left',
                }}
                keepMounted
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'left',
                }}
                open={Boolean(anchorEl)}
                onClose={handleClose}
                sx={{
                    color: 'primary'
                }}
                MenuListProps={{
                    'aria-labelledby': 'menu-button-card-actions',
                }}
            >
                <MenuItem
                    sx={{
                        textAlign: 'center'
                    }}>
                    Edit
                </MenuItem>
                <MenuItem
                    onClick={handleDeletePlan}
                    sx={{
                        textAlign: 'center'
                    }}>
                    Delete
                </MenuItem>
            </Menu>
        </Box>
    )
}
