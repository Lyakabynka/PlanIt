import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Menu from "@mui/material/Menu";
import MenuIcon from "@mui/icons-material/Menu";
import Container from "@mui/material/Container";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import Tooltip from "@mui/material/Tooltip";
import MenuItem from "@mui/material/MenuItem";
import { useEffect, useState } from "react";
import { EnumUserRole, useAuthStore } from "../../entities";
import { Link } from "react-router-dom";
import { PlanItIcon } from "../../features";
// import './Navbar.module.scss'

interface INavItem {
    pathName: string;
    path: string;
}

export function Navbar() {

    const { role, isLoggedIn } = useAuthStore();

    const [mainNavItems, setMainNavItems] = useState<INavItem[]>([]);
    const [profileNavItems, setProfileNavItems] = useState<INavItem[]>([
        { pathName: 'Profile', path: '/user/profile' },
        { pathName: 'Logout', path: '/logout' }
    ]);

    const updateRoleNavbarItems = () => {
        switch (role) {
            case EnumUserRole.user:
                setMainNavItems([
                    { path: '/plans', pathName: 'Plans' },
                ])
                break;
            default:
                setMainNavItems([
                    { path: '/login', pathName: 'Login' },
                    { path: '/register', pathName: 'Register' },
                ])
                break;

        }
    }

    useEffect(() => {
        updateRoleNavbarItems();
    }, []);

    useEffect(() => {
        updateRoleNavbarItems();
    }, [role, isLoggedIn]);

    const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElNav(event.currentTarget);
    };
    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(
        null
    );
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(
        null
    );

    const handleCloseNavMenu = () => {
        setAnchorElNav(null);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    return (
        <AppBar position="static" sx={{ backgroundColor: 'primary.main' }}>
            <Container maxWidth='xl'>
                <Toolbar disableGutters>
                    <Box sx={{ display: { xs: 'none', md: 'contents' } }}>
                        <PlanItIcon />
                    </Box>
                    <Typography
                        variant="h6"
                        noWrap
                        component="a"
                        href="/"
                        sx={{
                            mr: 2,
                            display: { xs: 'none', md: 'flex' },
                            fontWeight: 600,
                            letterSpacing: '.2rem',
                            color: 'primary.contrastText',
                            textDecoration: 'none',
                            caretColor: 'transparent'
                        }}
                    >
                        PLANIT
                    </Typography>

                    <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
                        <IconButton
                            size="large"
                            aria-label="account of current user"
                            aria-controls="menu-appbar"
                            aria-haspopup="true"
                            onClick={handleOpenNavMenu}
                            color="inherit"
                        >
                            <MenuIcon />
                        </IconButton>
                        <Menu
                            id="menu-appbar"
                            anchorEl={anchorElNav}
                            anchorOrigin={{
                                vertical: 'bottom',
                                horizontal: 'left',
                            }}
                            keepMounted
                            transformOrigin={{
                                vertical: 'top',
                                horizontal: 'left',
                            }}
                            open={Boolean(anchorElNav)}
                            onClose={handleCloseNavMenu}
                            sx={{
                                display: { xs: 'block', md: 'none' },
                            }}
                        >
                            {mainNavItems.map((item) => (
                                <MenuItem
                                    key={item.pathName}
                                    // href={item.path}
                                    onClick={handleCloseNavMenu}>
                                    <Link
                                        style={{ textAlign: 'center', color: 'inherit', textDecoration: 'none' }}
                                        to={item.path}>
                                        {item.pathName}
                                    </Link>
                                </MenuItem>
                            ))}
                        </Menu>
                    </Box>
                    <Box sx={{ display: { xs: 'contents', md: 'none' } }}>
                        <PlanItIcon />
                    </Box>
                    <Typography
                        variant="h5"
                        noWrap
                        component="a"
                        href=""
                        sx={{
                            mr: 11,
                            display: { xs: 'flex', md: 'none' },
                            flexGrow: 1,
                            fontWeight: 700,
                            letterSpacing: '.3rem',
                            color: 'primary.contrastText',
                            textDecoration: 'none',
                        }}
                    >
                        PLANIT
                    </Typography>
                    <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
                        {mainNavItems.map((item) => (
                            <Button
                                key={item.pathName}
                                onClick={handleCloseNavMenu}
                                sx={{ my: 2, color: 'primary.contrastText', display: 'block' }}
                            >
                                <Link
                                    style={{ textAlign: 'center', color: 'inherit', textDecoration: 'none' }}
                                    to={item.path}>
                                    {item.pathName}
                                </Link>
                            </Button>
                        ))}
                    </Box>


                    {isLoggedIn &&
                        <Box sx={{ flexGrow: 0 }}>
                            <Tooltip title="Open settings">
                                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                                    <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg" />
                                </IconButton>
                            </Tooltip>
                            <Menu
                                sx={{ mt: '45px' }}
                                id="menu-appbar"
                                anchorEl={anchorElUser}
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                open={Boolean(anchorElUser)}
                                onClose={handleCloseUserMenu}
                            >
                                {profileNavItems.map((item) => (
                                    <MenuItem
                                        key={item.pathName}
                                        onClick={handleCloseUserMenu}>
                                        <Link
                                            style={{ textAlign: 'center', color: 'inherit', textDecoration: 'none' }}
                                            to={item.path}>
                                            {item.pathName}
                                        </Link>
                                    </MenuItem>
                                ))}
                            </Menu>
                        </Box>
                    }
                </Toolbar>
            </Container>
        </AppBar>
    );
}