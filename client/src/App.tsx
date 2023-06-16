import { MantineProvider, Flex, createStyles } from '@mantine/core';
import './fonts/Fonts.css';
import { TopBar } from './components/TopBar/TopBar';
import { Resume } from './components/Resume/Resume';
import { Profile } from './components/Profile/Profile';
import GameButton from './components/GameButton/GameButton';
import useIsMobile from './hooks/useIsMobile';
import { useState } from 'react';
import { Footer } from './components/Footer/Footer';
const useStyles = createStyles(() => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    gap: 20,
    height: '100%',
  },
  profileGame: {
    width: '80%',
    justifyContent: 'space-between',
    alignItems: 'center',
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 20,
  },
  summaryContainer: {
    display: 'flex',
    flexDirection: 'row',
    width: '80%',
  },
  gameBox: {
    height: '0',
  },
}));
const App = () => {
  const [isPlaying, setIsPlaying] = useState<boolean>(false);
  const [isGameLoaded, setIsGameLoaded] = useState<boolean>(false);
  const isMobile = useIsMobile();
  const { classes } = useStyles();
  return (
    <Flex className={classes.root}>
      <TopBar isMobile={isMobile} />
      <Flex className={classes.profileGame}>
        <Profile />
        <GameButton
          isLoaded={isGameLoaded}
          isPlaying={isPlaying}
          onClick={setIsPlaying}
        />
      </Flex>
      <Resume isPlaying={isPlaying} setIsGameLoaded={setIsGameLoaded} />
      <Footer />
    </Flex>
  );
};

const RayDevs = () => {
  return (
    <MantineProvider
      withGlobalStyles
      withNormalizeCSS
      theme={{
        spacing: {
          xl: '100',
        },
        colorScheme: 'dark',
        fontFamily: 'JetbrainsMonoLight',
      }}>
      <App />
    </MantineProvider>
  );
};

export default RayDevs;
