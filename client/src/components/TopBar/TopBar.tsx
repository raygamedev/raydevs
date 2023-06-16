import {
  Text,
  createStyles,
  Header,
  Container,
  Group,
  rem,
} from '@mantine/core';
import {
  IconBrandGithub,
  IconBrandLinkedin,
  IconFileDescription,
} from '@tabler/icons-react';
import { downloadFile } from '../../api';
import TopBarButton from './TopBarButton';

const HEADER_HEIGHT = rem(60);

const useStyles = createStyles((theme) => ({
  root: {
    width: '100%',
    position: 'relative',
    zIndex: 1,
  },

  header: {
    display: 'flex',
    justifyContent: 'space-around',
    alignItems: 'center',
    maxWidth: '100%',
    width: '100%',
    height: '100%',
  },
  title: {
    fontFamily: 'Retro',
    fontSize: rem(20),
    color: theme.colorScheme == 'dark' ? theme.white : theme.colors.dark[7],
  },
}));

interface TopBarProps {
  isMobile: boolean;
}

export function TopBar({ isMobile }: TopBarProps) {
  const { classes } = useStyles();

  return (
    <Header height={HEADER_HEIGHT} className={classes.root}>
      <Container className={classes.header}>
        <Text className={classes.title}>Raydevs</Text>
        <Group noWrap>
          <TopBarButton
            text={'GitHub'}
            icon={<IconBrandGithub size={rem(20)} />}
            color={'gray'}
            link={'https://github.com/raygamedev'}
            isMobile={isMobile}
          />
          <TopBarButton
            text={'LinkedIn'}
            icon={<IconBrandLinkedin size={rem(20)} />}
            color={'gray'}
            link={'https://www.linkedin.com/in/ray-dev/'}
            isMobile={isMobile}
          />
          <TopBarButton
            text={'CV'}
            icon={<IconFileDescription size={rem(20)} />}
            color={'violet'}
            onClick={downloadFile}
            isMobile={isMobile}
          />
        </Group>
      </Container>
    </Header>
  );
}
