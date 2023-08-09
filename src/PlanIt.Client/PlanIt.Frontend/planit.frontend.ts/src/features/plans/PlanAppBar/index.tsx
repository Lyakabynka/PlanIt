import { AppBar } from '@mui/material';
import { useState } from 'react';
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import Container from "@mui/material/Container";
import Button from "@mui/material/Button";
import { AddPlanDialog } from '../AddPlanDialog';

export const PlanAppBar = () => {

  const [open, setOpen] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  }

  return (
    <>

      <AppBar position='relative'
        sx={{ backgroundColor: 'primary.light' }}>
        <Toolbar disableGutters>
          <Container maxWidth='xl'>
            <Box sx={{ flexGrow: 1, display: 'flex' }}>
              <Button
                onClick={handleOpen}
                sx={{ my: 2, color: 'primary.contrastText', display: 'block', textTransform: 'none' }}
              >
                Add new plan
              </Button>
            </Box>
          </Container>
        </Toolbar>
      </AppBar>

      <AddPlanDialog open={open} setOpen={setOpen} />
    </>
  )
}
