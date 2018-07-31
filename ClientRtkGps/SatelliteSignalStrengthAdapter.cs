using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ClientRtkGps
{
    enum SatelliteType {
        GPS,
        Glonass,
        Beidou
    }

    class SatelliteSignal
    {
        public int prn;
        public int value;
    }

    class SatelliteSignalStrengthAdapter
    {
        private Panel container;
        private SortedDictionary<SatelliteType, List<SatelliteSignal>> signals = 
            new SortedDictionary<SatelliteType, List<SatelliteSignal>>();
        private List<View> views = new List<View>();

        private class View
        {
            private Label label;
            private Label valueLabel;
            private CustomProgressBar progressBar;

            private int TOP_PADDING = 40;
            private const int DEFAULT_HEIGHT = 30;

            public View(Panel container, int index)
            {
                int x = 0;
                int y = index * DEFAULT_HEIGHT + TOP_PADDING;

                label = new Label()
                {
                    Location = new System.Drawing.Point(x, y),
                    Width = 60
                };

                valueLabel = new Label()
                {
                    Location = new System.Drawing.Point(label.Location.X + label.Width + 5, y),
                    Width = 30
                };

                progressBar = new CustomProgressBar()
                {
                    Location = new System.Drawing.Point(valueLabel.Location.X + valueLabel.Width + 5, y)
                };

                container.Controls.Add(label);
                container.Controls.Add(valueLabel);
                container.Controls.Add(progressBar);
            }

            public bool Visibile
            {
                set
                {
                    label.Visible = value;
                    progressBar.Visible = value;
                    valueLabel.Visible = value;
                }

                get
                {
                    return label.Visible || progressBar.Visible || valueLabel.Visible;
                }
            }

            public string Text
            {
                set
                {
                    label.Text = value;
                }

                get
                {
                    return label.Text;
                }
            }

            public int Value
            {
                set
                {
                    progressBar.Value = value;
                    valueLabel.Text = value.ToString();
                }

                get
                {
                    return progressBar.Value;
                }
            }
        }

        public SatelliteSignalStrengthAdapter(Panel container)
        {
            this.container = container ?? throw new ArgumentNullException("Container may not be null");
        }

        public void SetSignals(SatelliteType type, List<SatelliteSignal> signals)
        {
            this.signals[type] = signals;
            Draw();
        }

        private void Draw()
        {
            int signalsCount = signals.Aggregate(0, (accumulator, element) => 
                accumulator + element.Value?.Count ?? 0
            );

            for (int i = 0; i < signalsCount; i++)
            {
                if (views.Count <= i)
                {
                    views.Add(new View(container, i));
                }

                var view = views[i];

                int searchPos = 0;
                SatelliteSignal signal = null;
                SatelliteType? type = null;
                foreach (var signalType in signals.Keys)
                {
                    var list = signals[signalType];
                    int topLimit = searchPos + (list?.Count ?? 0);
                    if (i < topLimit && list != null)
                    {
                        signal = list[i - searchPos];
                        type = signalType;
                        break;
                    }

                    searchPos = topLimit;
                }

                if (signal != null && type != null)
                {
                    string satelliteTypeName;
                    switch (type)
                    {
                        case SatelliteType.GPS:
                        case SatelliteType.Glonass:
                        case SatelliteType.Beidou:
                            satelliteTypeName = type.ToString();
                            break;
                        default:
                            satelliteTypeName = "Satellite";
                            break;
                    }

                    view.Text = $"{satelliteTypeName}{signal.prn}";
                    view.Value = signal.value;
                    view.Visibile = true;
                }
            }

            for (int i = signalsCount; i < views.Count; i++)
            {
                views[i].Visibile = false;
            }

        }

    }
}
