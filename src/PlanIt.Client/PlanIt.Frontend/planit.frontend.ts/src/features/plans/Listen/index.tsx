import React, { useEffect } from 'react'
import MicIcon from '@mui/icons-material/Mic';
import MicOffIcon from '@mui/icons-material/MicOff';
import { IconButton } from '@mui/material';
import { usePlanStore } from '../../../pages/plan/usePlanStore';
import SpeechRecognition, { useSpeechRecognition } from 'react-speech-recognition';
import { EnumPlanType, EnumScheduledPlanType, IPlan, ICreateScheduledPlanRequest } from '../../../entities';
import BlockIcon from '@mui/icons-material/Block';

function generateCommands(
    plans: IPlan[],
    schedulePlan: (requestModel: ICreateScheduledPlanRequest) => Promise<void>) {

    const commands = plans.map((plan) => {

        switch (plan.type) {
            case EnumPlanType.volume:
                return ({
                    command: `${plan.name} *`,
                    callback: (volume: string) => {
                        try {
                            const number = Number.parseInt(volume);
                            schedulePlan({
                                planId: plan.id,
                                type: EnumScheduledPlanType.instant,
                                cronExpressionUtc: null,
                                executeUtc: null,
                                arguments: volume
                            })
                        }
                        catch (ex) {
                            //Handle the exception
                            console.log(`'${volume}' is not a number.`);
                        }
                    }
                })
            // case EnumPlanType.weatherCommand:
            //     return ({
            //         command: `${plan.name} *`,
            //         callback: (city: string) => {
            //             schedulePlan(plan.id, {
            //                 type: EnumScheduledPlanType.instant,
            //                 cronExpressionUtc: null,
            //                 executeUtc: null,
            //                 arguments: city
            //             })
            //         }
            //     })
            default:
                return ({
                    command: `${plan.name}`,
                    callback: () => {
                        schedulePlan({
                            planId: plan.id,
                            type: EnumScheduledPlanType.instant,
                            cronExpressionUtc: null,
                            executeUtc: null,
                            arguments: null
                        })
                    }
                });
        }
    });

    return commands;
}

export const Listen = () => {

    const { plans, createScheduledPlan } = usePlanStore();

    const commands = generateCommands(plans, createScheduledPlan);

    const { transcript,
        interimTranscript,
        finalTranscript,
        resetTranscript,
        listening } = useSpeechRecognition({ commands });


    const handleClick = () => {
        if (!listening) {
            SpeechRecognition.startListening({continuous: true});
        }
        else {
            SpeechRecognition.stopListening();
            resetTranscript();
        }
    }
    return (
        <>
            {SpeechRecognition.browserSupportsSpeechRecognition() ?
                <IconButton
                    size='large'
                    sx={{
                        backgroundColor: 'primary.main',
                        color: 'primary.contrastText',
                        '&:hover': { backgroundColor: 'primary.dark' },
                    }}
                    onClick={handleClick}
                >
                    {listening ? <MicIcon /> : <MicOffIcon />}
                </IconButton>
                :
                <IconButton
                    size='large'
                    sx={{
                        backgroundColor: 'primary.main',
                        color: 'primary.contrastText',
                        '&:hover': { backgroundColor: 'primary.dark' },
                    }}>
                    <BlockIcon />
                </IconButton>}
        </>
    )
}
