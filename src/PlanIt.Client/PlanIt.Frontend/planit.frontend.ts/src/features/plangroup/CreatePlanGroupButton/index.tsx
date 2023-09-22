import { IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import React from 'react'
import { CreatePlanGroupDialog } from '../CreatePlanGroupDialog';

export const CreatePlanGroupButton = () => {
  const [createPlanGroupDialogOpen, setCreatePlanGroupDialogOpen] = React.useState<boolean>(false);

  const handleCreatePlanGroupDialogOpen = (e: React.MouseEvent<HTMLButtonElement>) => {
      e.preventDefault();
      setCreatePlanGroupDialogOpen(true);
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
              onClick={handleCreatePlanGroupDialogOpen}>
              <AddIcon />
          </IconButton>
          <CreatePlanGroupDialog open={createPlanGroupDialogOpen} setOpen={setCreatePlanGroupDialogOpen} />
      </>
  )
}
