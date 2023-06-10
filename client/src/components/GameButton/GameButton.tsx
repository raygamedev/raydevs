import {
  Button,
  Modal,
  createStyles,
  Text,
  Group,
  Flex,
  Loader,
} from '@mantine/core';
import { IconPlayerPause, IconPlayerPlay } from '@tabler/icons-react';
import useIsMobile from '../../hooks/useIsMobile';

const BUTTON_WIDTH = 300;

const useStyles = createStyles((theme) => ({
  root: {
    backgroundColor:
      theme.colorScheme === 'dark' ? theme.colors.dark[6] : theme.white,
    boxShadow: theme.shadows.md,
    width: BUTTON_WIDTH,
    height: 100,
    fontSize: 37,
    borderRadius: 10,
    justifyContent: 'space-around',
    fontFamily: 'Retro',
    border: `${theme.spacing.xl} solid ${
      theme.colorScheme === 'dark' ? theme.colors.dark[4] : theme.colors.gray[1]
    }`,
  },
  container: {
    width: '100%',
    maxWidth: window.innerWidth <= 768 ? 768 : BUTTON_WIDTH,
    justifyContent: 'center',
  },
  text: {
    color: theme.colorScheme == 'dark' ? theme.white : theme.colors.dark[7],
    alignContent: 'center',
    justifyContent: 'center',
    alignItems: 'center',
    textAlign: 'center',
    width: '60%',
  },
}));
interface GameButtonProps {
  isPlaying: boolean;
  onClick: (value: boolean) => void;
  isLoaded: boolean;
}

const GameButton = ({ isPlaying, onClick, isLoaded }: GameButtonProps) => {
  const isMobile = useIsMobile();
  const { classes } = useStyles();
  if (isMobile) {
    return (
      <Group className={classes.container}>
        <Modal
          radius={15}
          opened={isPlaying && isMobile}
          onClose={() => onClick(!isPlaying)}>
          <Flex
            justify="space-between"
            direction="column"
            align="center"
            gap={20}
            mb="xl"
            mt="xl">
            <Text className={classes.text}>
              {"Pleb's Journey isnt available on Mobile yet"}
            </Text>
            <Text size="sm" color="dimmed">
              Please open in a desktop browser
            </Text>
            <Button color="violet" onClick={() => onClick(!isPlaying)}>
              Close
            </Button>
          </Flex>
        </Modal>
        <Button
          onClick={() => onClick(!isPlaying)}
          className={classes.root}
          variant="gradient"
          rightIcon={<IconPlayerPlay stroke={4} size={50} />}
          gradient={{ from: 'purple', to: 'indigo' }}>
          try me
        </Button>
      </Group>
    );
  }
  if (!isLoaded) {
    return (
      <Button
        disabled={!isLoaded}
        onClick={() => onClick(!isPlaying)}
        className={classes.root}
        variant="gradient"
        gradient={{ from: 'purple', to: 'indigo' }}>
        <Loader color={'white'} size="lg" />
      </Button>
    );
  }
  if (isPlaying) {
    return (
      <Button
        disabled={isPlaying && isMobile}
        onClick={() => onClick(!isPlaying)}
        className={classes.root}
        variant="gradient"
        gradient={{ from: 'red', to: 'purple' }}
        rightIcon={<IconPlayerPause size={50} />}>
        pause
      </Button>
    );
  }
  return (
    <Button
      disabled={isPlaying && isMobile}
      onClick={() => onClick(!isPlaying)}
      className={classes.root}
      variant="gradient"
      rightIcon={<IconPlayerPlay stroke={4} size={50} />}
      gradient={{ from: 'purple', to: 'indigo' }}>
      try me
    </Button>
  );
};

export default GameButton;
