import React, { useState, useEffect } from 'react';
import { 
  ThemeProvider, createTheme, CssBaseline, Container, Typography, 
  Box, Card, CardContent, Grid, Button, MenuItem, Select, 
  FormControl, InputLabel, Divider, CircularProgress, Alert 
} from '@mui/material';
import SportsSoccerIcon from '@mui/icons-material/SportsSoccer';

// Definerer fotball-inspirert fargetema (mørk modus med gressgrønn og gull)
const theme = createTheme({
  palette: {
    mode: 'dark',
    primary: { main: '#2e7d32' }, // Gressgrønn
    secondary: { main: '#ffd700' }, // Gull
    background: { default: '#0a192f', paper: '#112240' },
  },
  typography: { fontFamily: 'Roboto, sans-serif' },
});

export default function App() {
  const API_BASE = 'http://localhost:63516';

  // State-håndtering
  const [teams, setTeams] = useState([]);
  const [matches, setMatches] = useState([]);
  const [groups, setGroups] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(false);

  // Brukerens innsendte gjett
  const [predictions, setPredictions] = useState({
    groupPlacements: {}, // { groupId: { teamId: plassering } }
    matchWinners: {}      // { matchId: winnerTeamId }
  });

  // Henter data fra backend ved oppstart
  useEffect(() => {
    Promise.all([
      fetch(`${API_BASE}/team`).then(res => res.json()),
      fetch(`${API_BASE}/match`).then(res => res.json()),
      fetch(`${API_BASE}/groupPlay`).then(res => res.json())
    ])
    .then(([teamsData, matchesData, groupsData]) => {
      setTeams(teamsData);
      setMatches(matchesData);
      setGroups(groupsData);
      setLoading(false);
    })
    .catch(err => {
      console.error(err);
      setError('Kunne ikke hente data fra serveren.');
      setLoading(false);
    });
  }, []);

  // Oppdaterer lagets plassering i en gruppe (1-4)
  const handleGroupPrediction = (groupId, teamId, placement) => {
    setPredictions(prev => ({
      ...prev,
      groupPlacements: {
        ...prev.groupPlacements,
        [groupId]: {
          ...(prev.groupPlacements[groupId] || {}),
          [teamId]: parseInt(placement)
        }
      }
    }));
  };

  // Oppdaterer vinner av en utslagskamp
  const handleMatchPrediction = (matchId, winnerId) => {
    setPredictions(prev => ({
      ...prev,
      matchWinners: {
        ...prev.matchWinners,
        [matchId]: winnerId
      }
    }));
  };

  // Sender inn alle gjett til backend via POST
  const handleSubmit = async () => {
    setError(null);
    setSuccess(false);

    try {
      // POST gruppespill-gjett til /groupPlay
      await fetch(`${API_BASE}/groupPlay`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(predictions.groupPlacements)
      });

      // POST utslagskamper-gjett til /match
      await fetch(`${API_BASE}/match`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(predictions.matchWinners)
      });

      setSuccess(true);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    } catch (err) {
      setError('Feil under innsending av kuppongen.');
    }
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
        <CircularProgress color="secondary" />
      </Box>
    );
  }

  // Grupperer utslagskamper basert på type (antar match-objektet har et 'type'-felt)
  const matchStages = {
    '16-delsfinale': { points: 15, list: matches.filter(m => m.type === '16-delsfinale') },
    'Kvartfinale': { points: 20, list: matches.filter(m => m.type === 'Kvartfinale') },
    'Semifinale': { points: 40, list: matches.filter(m => m.type === 'Semifinale') },
    'Bronsefinale': { points: 50, list: matches.filter(m => m.type === 'Bronsefinale') },
    'Finale': { points: 100, list: matches.filter(m => m.type === 'Finale') },
  };

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Container maxWidth="md" sx={{ py: 5 }}>
        
        {/* Header */}
        <Box textAlign="center" mb={6}>
          <SportsSoccerIcon sx={{ fontSize: 60, color: 'secondary.main', mb: 2 }} />
          <Typography variant="h2" component="h1" fontWeight="bold" color="white" gutterBottom>
            Fotballkonkurranse
          </Typography>
          <Typography variant="h6" color="textSecondary">
            Gjett resultatene for fotball-VM og samle poeng!
          </Typography>
        </Box>

        {/* Meldinger */}
        {error && <Alert severity="error" sx={{ mb: 4 }}>{error}</Alert>}
        {success && <Alert severity="success" sx={{ mb: 4 }}>Kuppongen din er lagret! Lykke til!</Alert>}

        {/* DEL 1: Gruppespill */}
        <Typography variant="h4" color="secondary" gutterBottom sx={{ mt: 4, fontWeight: 'bold' }}>
          Gruppespill <Typography component="span" variant="body1" color="textSecondary">(5 poeng per riktig plassering)</Typography>
        </Typography>
        <Divider sx={{ mb: 3, bg: 'primary.main' }} />
        
        <Grid container spacing={3}>
          {groups.map((group) => (
            <Grid item xs={12} sm={6} key={group.id}>
              <Card sx={{ borderLeft: '5px solid #2e7d32' }}>
                <CardContent>
                  <Typography variant="h6" fontWeight="bold" gutterBottom>{group.name}</Typography>
                  {/* Antar group.teams inneholder lag-IDer eller lag-objekter */}
                  {group.teams?.map((teamId) => {
                    const team = teams.find(t => t.id === teamId);
                    if (!team) return null;
                    return (
                      <Box key={team.id} display="flex" alignItems="center" justifyContent="space-between" my={1.5}>
                        <Typography>{team.name}</Typography>
                        <FormControl size="small" sx={{ width: 100 }}>
                          <InputLabel>Plass</InputLabel>
                          <Select
                            label="Plass"
                            value={predictions.groupPlacements[group.id]?.[team.id] || ''}
                            onChange={(e) => handleGroupPrediction(group.id, team.id, e.target.value)}
                          >
                            {[1, 2, 3, 4].map(num => (
                              <MenuItem key={num} value={num}>{num}. plass</MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </Box>
                    );
                  })}
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>

        {/* DEL 2: Utslagskamper */}
        {Object.entries(matchStages).map(([stageName, stageInfo]) => (
          stageInfo.list.length > 0 && (
            <Box key={stageName} sx={{ mt: 6 }}>
              <Typography variant="h4" color="secondary" gutterBottom sx={{ fontWeight: 'bold' }}>
                {stageName} <Typography component="span" variant="body1" color="textSecondary">({stageInfo.points} poeng)</Typography>
              </Typography>
              <Divider sx={{ mb: 3 }} />

              <Grid container spacing={2}>
                {stageInfo.list.map((match) => {
                  const homeTeam = teams.find(t => t.id === match.homeTeamId);
                  const awayTeam = teams.find(t => t.id === match.awayTeamId);
                  return (
                    <Grid item xs={12} key={match.id}>
                      <Card>
                        <CardContent display="flex" sx={{ alignItems: 'center', py: 2 }}>
                          <Grid container alignItems="center" spacing={2}>
                            <Grid item xs={5} textAlign="right">
                              <Typography variant="h6">{homeTeam?.name || 'Hjemmelag'}</Typography>
                            </Grid>
                            <Grid item xs={2} textAlign="center">
                              <Typography variant="body2" color="textSecondary">VS</Typography>
                            </Grid>
                            <Grid item xs={5} textAlign="left">
                              <Typography variant="h6">{awayTeam?.name || 'Bortelag'}</Typography>
                            </Grid>
                          </Grid>
                          
                          <Box display="flex" justifyContent="center" mt={2}>
                            <FormControl size="small" sx={{ width: 200 }}>
                              <InputLabel>Velg vinner</InputLabel>
                              <Select
                                label="Velg vinner"
                                value={predictions.matchWinners[match.id] || ''}
                                onChange={(e) => handleMatchPrediction(match.id, e.target.value)}
                              >
                                <MenuItem value={homeTeam?.id}>{homeTeam?.name}</MenuItem>
                                <MenuItem value={awayTeam?.id}>{awayTeam?.name}</MenuItem>
                              </Select>
                            </FormControl>
                          </Box>
                        </CardContent>
                      </Card>
                    </Grid>
                  );
                })}
              </Grid>
            </Box>
          )
        ))}

        {/* Innsending */}
        <Box textAlign="center" mt={6} mb={4}>
          <Button 
            variant="contained" 
            color="secondary" 
            size="large" 
            onClick={handleSubmit}
            sx={{ px: 6, py: 1.5, fontSize: '1.2rem', fontWeight: 'bold', color: '#0a192f' }}
          >
            Send inn kuppong
          </Button>
        </Box>

      </Container>
    </ThemeProvider>
  );
}
