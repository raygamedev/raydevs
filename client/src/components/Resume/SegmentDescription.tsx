import { Card, Text, Badge, Group, createStyles, Divider } from '@mantine/core';
import { Colors } from '../../styles/colors';
import { SegmentDescriptionData } from './types';

const useStyles = createStyles((theme) => ({
  root: {
    backgroundColor:
      theme.colorScheme === 'dark' ? theme.colors.dark[7] : theme.white,
    borderRadius: 30,
    width: '60%',
    height: '75%',
    overflowY: 'scroll',
  },

  title: {
    color: Colors.LIGHT_PURPLE,
    textAlign: 'left',
    fontWeight: 700,
    fontFamily: `Greycliff CF, ${theme.fontFamily}`,
    lineHeight: 1.2,
  },

  body: {
    padding: theme.spacing.md,
  },

  group: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    gap: 10,
  },

  textDivider: {
    backgroundColor: 'violet',
    width: '70%',
    borderRadius: 10,
    alignSelf: 'center',
    height: 1,
  },
}));

interface SegmentDescriptionProps {
  segmentDescriptionList: SegmentDescriptionData[];
  isMobile: boolean;
}

export const SegmentDescription = ({
  segmentDescriptionList,
  isMobile
}: SegmentDescriptionProps) => {
  const { classes } = useStyles();
  return isMobile ? (
    <Card radius={10} style={{ width: '97%' }}>
      {segmentDescriptionList.map(
        (item: SegmentDescriptionData, index: number) => (
          <>
            <Group style={{ gap: 3 }} position="left" mt="md" mb="xs">
              {item.badge.map((badge) => (
                <Badge key={badge} size="lg" color="violet" variant="light">
                  {badge}
                </Badge>
              ))}
            </Group>
            <Group className={classes.group}>
              {item.text.map((text, index) => (
                <Group key={index} noWrap position="left">
                  <Text>•</Text>
                  <Text size="sm" color="dimmed">
                    {text}
                  </Text>
                </Group>
              ))}
              {index !== segmentDescriptionList.length - 1 && (
                <Divider className={classes.textDivider} />
              )}
            </Group>
          </>
        ),
      )}
    </Card>
  ) : (
    <Card
      className={classes.root}
      shadow="sm"
      padding="lg"
      radius={30}
      withBorder>
      {segmentDescriptionList.map((item: SegmentDescriptionData) => (
        <>
          <Group position="left" mt="md" mb="xs">
            {Array.isArray(item.badge) ? (
              item.badge.map((badge) => (
                <Badge key={badge} size="lg" color="violet" variant="light">
                  {badge}
                </Badge>
              ))
            ) : (
              <Badge size="lg" color="violet" variant="light">
                {item.badge}
              </Badge>
            )}
          </Group>
          {Array.isArray(item.text) ? (
            item.text.map((text, index) => (
              <Group noWrap key={index} position="left">
                <Text>•</Text>
                <Text size="sm" color="dimmed">
                  {text}
                </Text>
              </Group>
            ))
          ) : (
            <Group noWrap position="left" mt="md" mb="xs">
              <Text>•</Text>
              <Text size="sm" color="dimmed">
                {item.text}
              </Text>
            </Group>
          )}
        </>
      ))}
    </Card>
  );
};
