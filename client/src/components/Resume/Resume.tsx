import { motion } from 'framer-motion';
import {
  Avatar,
  Box,
  createStyles,
  Flex,
  Group,
  NavLink,
  SegmentedControlItem,
  Text,
} from '@mantine/core';
import { SegmentControl } from './SegmentControl';
import { SegmentItem } from './SegmentItem';
import InspektoLogo from '../../art/InspektoLogo.png';
import { useState } from 'react';
import { ResumeCardType } from './enums';
import { ResumeData, ResumeKeys } from './types';
import { SegmentDescription } from './SegmentDescription';
import Summary from './Summary';
import useIsMobile from '../../hooks/useIsMobile';
import { RaydevsGame } from '../RaydevsGame/RaydevsGame';

const resumeData: ResumeData = {
  [ResumeKeys.SoftwareDeveloper]: {
    segmentItem: {
      title: 'Software Developer',
      startDate: 'April 2020',
      endDate: 'Current',
      company: {
        name: 'Inspekto',
        avatar: InspektoLogo,
      },
    },
    segmentDescription: [
      {
        badge: ['React', 'TypeScript', 'Python', 'Flask', 'FastAPI', 'MongoDB'],
        text: [
          'Full-stack development using Python, React, TypeScript, MongoDB resulting in a highly responsive and user-friendly application.',
          'Full responsibility for features, from design, development, testing to deployment for both frontend and backend.',
        ],
      },
      {
        badge: ['Docker', 'Gitlab CI/CD'],
        text: ['Implemented DevOps CI/CD practices on Linux, Docker, GitlabCI'],
      },
      {
        badge: ['Ansible', 'Python', 'Bash', 'Preseed', 'Linux'],
        text: [
          'Responsible for the deployment of the product on Linux on-prem machines, including deciding on tools and designing the deployment process.',
          'Created a new deployment process for our product from scratch using Ansible, Python, Bash, and Preseed for automating boot installtion.',
          'Streamlined the deployment pipeline by implementing automated scripts and configuration management tools.',
          'Created a new deployment process for our dev machines using Ansible, Python, Bash, and Preseed for automating installtion.',
          'Significantly reduced deployment times and human errors for both dev and prod environments.',
        ],
      },
    ],
  },
  [ResumeKeys.QaAutomation]: {
    segmentItem: {
      title: 'QA Automation',
      startDate: 'April 2019',
      endDate: 'April 2020',
      company: {
        name: 'Inspekto',
        avatar: InspektoLogo,
      },
    },
    segmentDescription: [
      {
        badge: ['Node.JS', 'Puppeteer'],
        text: ['Development UI Automation using NodeJS and Puppeteer'],
      },
      {
        badge: ['python'],
        text: ['Development of Automation framework using Python from scratch'],
      },
      {
        badge: ['Jenkins', 'Python'],
        text: ['Development of CI/CD pipeline using Jenkins and Python'],
      },
      {
        badge: ['Misc'],
        text: [
          'Responsible for deciding on infastructure and tools for automation',
          "During this period I've studied and learned about automation, CI/CD, DevOps, and Software Development in my free time and I've been able to apply this knowledge to my work",
        ],
      },
    ],
  },
  [ResumeKeys.DataTeamMember]: {
    segmentItem: {
      title: 'Data Team Member',
      startDate: 'May 2018',
      endDate: 'April 2019',
      company: {
        name: 'Inspekto',
        avatar: InspektoLogo,
      },
    },
    segmentDescription: [
      {
        badge: ['Python'],
        text: [
          'Developed automated tools to improve efficiency and accuracy in data analysis processes.',
          'Conducted data analysis for the Algorithm team, creating visualizations and analyzing data using heatmaps, CSV files, and basic Python scripting.',
        ],
      },
      {
        badge: ['Git'],
        text: [
          'Learned Git fundamentals, enabling a better understanding of the development environment, and for testing ML models.',
        ],
      },
      {
        badge: ['Linux'],
        text: [
          'Acquired Linux fundamentals, enabling a better understanding of the development environment',
        ],
      },
    ],
  },
};

const useStyles = createStyles((theme) => ({
  root: {
    position: 'relative',
    backgroundColor:
      theme.colorScheme === 'dark' ? theme.colors.dark[6] : theme.white,
    boxShadow: theme.shadows.md,
    width: '80%',
    flexGrow: 1,
    borderRadius: 30,
    flexDirection: 'column',
    justifyContent: 'space-around',
    border: `${theme.spacing.xl} solid ${
      theme.colorScheme === 'dark' ? theme.colors.dark[4] : theme.colors.gray[1]
    }`,
  },
  segment: {
    width: '100%',
    minHeight: 500,
    height: '100%',
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-around',
  },
  div: {
    height: '100%',
  },
}));
interface ResumeProps {
  isPlaying: boolean;
  setIsGameLoaded: (isLoaded: boolean) => void;
}

export const Resume = ({ isPlaying, setIsGameLoaded }: ResumeProps) => {
  const isMobile = useIsMobile();
  const { classes } = useStyles();
  const [active, setActive] = useState<ResumeCardType>(
    ResumeCardType.SoftwareDeveloper,
  );

  const extractSegmentItems = (
    resumeData: ResumeData,
  ): SegmentedControlItem[] => {
    const segmentedItems: SegmentedControlItem[] = [];
    for (const key in resumeData) {
      const segment: SegmentedControlItem = {
        label: (
          <SegmentItem
            segmentData={resumeData[key as ResumeCardType].segmentItem}
          />
        ),
        value: key as ResumeCardType,
      };
      segmentedItems.push(segment);
    }
    return segmentedItems;
  };

  return isMobile ? (
    <>
      <Summary />
      <Box w={'100%'}>
        {Object.keys(resumeData).map((key) => {
          const item = resumeData[key as ResumeCardType].segmentItem;
          return (
            <NavLink
              key={key}
              label={
                <Group>
                  <Text>{item.title}</Text>
                  <Text align="left" size="xs" color="dimmed">
                    {item.startDate} - {item.endDate}
                  </Text>
                </Group>
              }
              icon={<Avatar size="1rem" src={item.company.avatar} />}
              childrenOffset={28}>
              <SegmentDescription
                isMobile={isMobile}
                segmentDescriptionList={
                  resumeData[key as ResumeCardType].segmentDescription
                }
              />
            </NavLink>
          );
        })}
      </Box>
    </>
  ) : (
    <Flex
      className={classes.root}
      style={{ minHeight: isPlaying ? 'max-content' : 900 }}>
      <RaydevsGame isPlaying={isPlaying} setIsGameLoaded={setIsGameLoaded} />
      <motion.div
        animate={{
          x: 0,
          y: 0,
          scale: isPlaying ? 0 : 1,
          opacity: isPlaying ? 0 : 1,
          rotate: 0,
        }}>
        {!isPlaying && (
          <>
            <Summary />
            <Flex className={classes.segment}>
              <SegmentControl
                setActive={setActive}
                segmentItems={extractSegmentItems(resumeData)}
              />
              <SegmentDescription
                isMobile={isMobile}
                segmentDescriptionList={resumeData[active].segmentDescription}
              />
            </Flex>
          </>
        )}
      </motion.div>
    </Flex>
  );
};
