import { Box } from '@mui/material';
import React from 'react'
import Typewriter, { Options, TypewriterClass } from "typewriter-effect";

export const MainPage = () => {
    return (
        <div style={{
            marginLeft: '15vh',
            marginTop: '25vh',
            fontSize: 50,
            fontWeight: 600,
            letterSpacing: '.05rem',
            color: 'primary.main',
            textDecoration: 'none',
        }}>
            <Typewriter
                options={{
                    strings: ['PLANIT', 'Task Scheduler', 'Remote Control', 'Recurring Plans'],
                    autoStart: true,
                    loop: true,
                    delay: 150,
                    deleteSpeed: 100,
                }} />
        </div>
    )
}
